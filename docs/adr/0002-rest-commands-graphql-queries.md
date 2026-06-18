# ADR 0002: REST for Commands, GraphQL for Queries

## Status
Accepted

## Context
Write operations should stay explicit, auditable, and simple. Read operations need flexible shape for dashboards and nested views.

## Decision
- Use Minimal API REST endpoints exclusively for commands under /api/v1.
- Use HotChocolate GraphQL exclusively for query use cases.
- GraphQL resolvers call query handlers only.

## Consequences
- Clear transport boundaries by intent (write vs read).
- Rich read composition without write-side complexity.
- Avoids accidental command execution through GraphQL.
