using DeveloperRegistry.Api.Common;
using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Common.Time;
using DeveloperRegistry.Api.Common.Validation;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.Api.Features.Applications.CreateApplication;

public sealed class Handler(RegistryDbContext dbContext, IClock clock, IValidator<Command> validator)
{
    public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
    {
        await validator.ValidateRequestAsync(command, cancellationToken);

        var exists = await dbContext.Applications.AnyAsync(x => x.Name == command.Name, cancellationToken);
        if (exists)
        {
            throw new ConflictException($"An application with the name '{command.Name}' already exists.");
        }

        var application = RegisteredApplication.Create(IdGenerator.NewId(), command.Name, command.Description, clock.UtcNow);
        dbContext.Applications.Add(application);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(application.Id, application.Name, application.Description, application.CreatedAtUtc);
    }
}
