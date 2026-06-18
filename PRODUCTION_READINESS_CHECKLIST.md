# Production Readiness Checklist

## Architecture
- [x] Vertical Slice Architecture by feature.
- [x] CQRS split: EF Core writes, Dapper reads.
- [x] No controllers, no MediatR, no repository abstraction.

## Security
- [x] API keys are generated randomly and persisted as hashes only.
- [x] Correlation IDs are included in logs for traceability.
- [ ] Add authentication/authorization for all command endpoints.
- [ ] Add rate limiting and abuse protection for key lifecycle endpoints.

## Data and Persistence
- [x] PostgreSQL schema managed by EF Core migrations.
- [x] Unique constraints on application name, owner email, and API key hash.
- [x] Soft-delete semantics via application archive flag.
- [x] Automatic migration execution in Development only.

## Observability
- [x] Serilog request + exception logging.
- [x] OpenTelemetry configured via Aspire service defaults.
- [x] Health endpoints: /health, /health/live, /health/ready.

## Quality
- [x] Central package management.
- [x] Build runs with warnings-as-errors.
- [x] Unit and integration test projects created.
- [ ] Enforce >= 90% coverage in CI gate.

## Operations
- [x] .NET Aspire AppHost orchestrates API + PostgreSQL.
- [x] GitHub Actions workflow for restore/build/test.
- [ ] Add deployment manifests (Bicep/Terraform/Kubernetes) for runtime environment.
