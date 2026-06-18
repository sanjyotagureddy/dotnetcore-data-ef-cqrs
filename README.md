# Developer Application Registry (.NET 10 Aspire Reference)

Developer Application Registry is a production-quality reference implementation that demonstrates modern .NET 10 architecture using Minimal APIs, Vertical Slice Architecture, CQRS, EF Core, Dapper, GraphQL, PostgreSQL, Aspire, OpenTelemetry, and test automation.

## Tech Stack
- .NET 10
- ASP.NET Core Minimal APIs
- .NET Aspire
- PostgreSQL
- Entity Framework Core 10 (commands/writes + migrations)
- Dapper (queries/reads)
- HotChocolate GraphQL (queries only)
- FluentValidation
- OpenTelemetry
- Serilog
- xUnit + Testcontainers

## Solution Structure

```text
src/
  DeveloperRegistry.AppHost
  DeveloperRegistry.ServiceDefaults
  DeveloperRegistry.Api

tests/
  DeveloperRegistry.UnitTests
  DeveloperRegistry.IntegrationTests
```

## Architecture Highlights
- Vertical slices under Features/ grouped by business capability.
- REST commands under /api/v1 only.
- GraphQL queries under /graphql only.
- EF Core handles all write-side operations and schema migrations.
- Dapper handles all read-side operations.

## API Surface

### Command REST Endpoints
- POST /api/v1/applications
- PUT /api/v1/applications/{id}
- DELETE /api/v1/applications/{id}
- POST /api/v1/applications/{id}/owners
- DELETE /api/v1/applications/{id}/owners/{ownerId}
- POST /api/v1/applications/{id}/apikeys
- POST /api/v1/apikeys/{id}/revoke
- POST /api/v1/apikeys/{id}/rotate
- POST /api/v1/applications/{id}/webhooks
- POST /api/v1/webhooks/{id}/enable
- POST /api/v1/webhooks/{id}/disable

### GraphQL Query Operations
- application(id: String!)
- applications(search: String)
- owner(id: String!)
- applicationDashboard(id: String!)

## Run Locally

### Prerequisites
- .NET SDK 10
- Docker Desktop (required for Aspire PostgreSQL container and integration tests)

### Start the app with Aspire
```bash
dotnet run --project src/DeveloperRegistry.AppHost
```

This starts:
- API service
- PostgreSQL container
- Aspire dashboard/telemetry wiring

## Build and Test
```bash
dotnet build DeveloperRegistry.slnx -warnaserror
dotnet test DeveloperRegistry.slnx
dotnet test tests/DeveloperRegistry.UnitTests/DeveloperRegistry.UnitTests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Threshold=90 /p:ThresholdType=line /p:ThresholdStat=total
```

Note: Integration tests require Docker to be running.

## Migrations
Initial migration is generated under:
- src/DeveloperRegistry.Api/Persistence/Migrations

To add future migrations:
```bash
dotnet-ef migrations add <MigrationName> --project src/DeveloperRegistry.Api/DeveloperRegistry.Api.csproj --startup-project src/DeveloperRegistry.Api/DeveloperRegistry.Api.csproj --output-dir Persistence/Migrations
```

## Observability
- Structured logging via Serilog
- OpenTelemetry defaults from Aspire ServiceDefaults
- Correlation ID propagation using X-Correlation-Id header
- Health endpoints:
  - /health
  - /health/live
  - /health/ready

## ADRs
See:
- docs/adr/0001-vertical-slice-cqrs.md
- docs/adr/0002-rest-commands-graphql-queries.md
- docs/adr/0003-efcore-writes-dapper-reads.md
