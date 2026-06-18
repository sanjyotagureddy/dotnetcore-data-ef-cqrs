# ADR 0003: EF Core Writes, Dapper Reads

## Status
Accepted

## Context
Write side benefits from aggregates, constraints, and change tracking. Read side benefits from targeted SQL and projection control.

## Decision
- EF Core is used only for command/write handlers and migrations.
- Dapper is used only for query/read handlers.
- Schema is managed entirely by EF Core migrations.

## Consequences
- Better write-side invariants and transaction handling.
- Predictable read performance and SQL control.
- Requires discipline to keep read/write boundaries enforced.
