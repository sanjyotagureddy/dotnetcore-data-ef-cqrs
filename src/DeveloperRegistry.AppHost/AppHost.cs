var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var registryDb = postgres.AddDatabase("registrydb");

builder.AddProject<Projects.DeveloperRegistry_Api>("api")
	.WithReference(registryDb)
	.WaitFor(registryDb);

builder.Build().Run();
