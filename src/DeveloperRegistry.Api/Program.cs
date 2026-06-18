using DeveloperRegistry.Api.Common.ProblemDetails;
using DeveloperRegistry.Api.Common.Time;
using DeveloperRegistry.Api.Features;
using DeveloperRegistry.Api.Features.GraphQL;
using DeveloperRegistry.Api.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using Serilog.Context;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
	loggerConfiguration
		.ReadFrom.Configuration(context.Configuration)
		.ReadFrom.Services(services)
		.Enrich.FromLogContext()
		.WriteTo.Console();
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var connectionString = builder.Configuration.GetConnectionString("registrydb")
	?? throw new InvalidOperationException("Connection string 'registrydb' is not configured.");

builder.Services.AddSingleton(_ => new NpgsqlDataSourceBuilder(connectionString).Build());
builder.Services.AddDbContext<RegistryDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IClock, SystemClock>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

foreach (var handlerType in typeof(Program).Assembly.GetTypes()
	.Where(type => type is { IsClass: true, IsAbstract: false, Name: "Handler" }))
{
	builder.Services.AddScoped(handlerType);
}

builder.Services.AddGraphQlQueries();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.CustomSchemaIds(type =>
	{
		if (string.IsNullOrWhiteSpace(type.FullName))
		{
			return type.Name;
		}

		return type.FullName.Replace('+', '.');
	});
});

builder.Services.AddHealthChecks()
	.AddDbContextCheck<RegistryDbContext>(tags: ["ready"]);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.Use(async (context, next) =>
{
	var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault() ?? context.TraceIdentifier;
	context.Response.Headers["X-Correlation-Id"] = correlationId;

	using (LogContext.PushProperty("CorrelationId", correlationId))
	{
		await next();
	}
});

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
	app.MapGet("/", () => Results.Redirect("/swagger"))
		.ExcludeFromDescription();

	await using var scope = app.Services.CreateAsyncScope();
	var dbContext = scope.ServiceProvider.GetRequiredService<RegistryDbContext>();
	await dbContext.Database.MigrateAsync();
}

var commands = app.MapGroup("/api/v1");
commands.MapCommandEndpoints();

app.MapGraphQL("/graphql");

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = x => x.Tags.Contains("live") });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = x => x.Tags.Contains("ready") });

app.MapDefaultEndpoints();

app.Run();

public partial class Program;
