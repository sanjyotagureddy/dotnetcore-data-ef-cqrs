# ADR 0001: Vertical Slice + CQRS

## Status
Accepted

## Context
The service is intentionally small but should demonstrate production-grade boundaries and maintainability patterns.

## Decision
Use Vertical Slice Architecture with explicit CQRS:
- Command slices own request, validation, handler, endpoint, and response.
- Query slices own query models and Dapper handlers.
- Features are organized by business capability, not layers.

## Consequences
- Higher local cohesion by feature.
- Lower accidental coupling across unrelated use cases.
- Easier evolution toward eventing/outbox patterns if needed.
