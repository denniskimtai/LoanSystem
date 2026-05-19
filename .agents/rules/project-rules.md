---
trigger: always_on
---

## Architecture
- Clean Architecture: Domain → Application → Infrastructure → Api
- Dependencies point inward only. Api and Infrastructure never reference each other.
- No business logic in Controllers, Repositories, or EF configurations.
- No EF Core types (DbContext, DbSet) outside Infrastructure layer.

## Entities
- All entities inherit BaseEntity (Id: Guid, CreatedAt: DateTime, UpdatedAt: DateTime, IsDeleted: bool)
- Soft delete only — never hard delete. Filter IsDeleted via global EF query filter.
- No public setters on domain entities. Use private setters + domain methods.
- Navigation properties are never null — initialize collections in constructor.

## CQRS
- Every operation is a MediatR Command or Query. Nothing calls repositories directly from controllers.
- Commands mutate state. Queries read state. Never mix both in one handler.
- Handlers return Result<T>. Never throw exceptions for business logic failures.
- One handler per file. File name matches handler name.

## Result Pattern
- All handlers return Result<T> or Result (non-generic for commands).
- Use Result.Success(), Result.Failure("reason") — never return null.
- Controllers map Result to HTTP responses. No business logic in controllers.

## Validation
- Every Command and Query has a corresponding FluentValidation validator.
- Validators registered automatically via assembly scanning.
- Validation runs via MediatR pipeline behavior before handler executes.
- Never validate manually inside a handler — if it reaches the handler, input is already valid.

## Database
- All EF configuration via Fluent API in IEntityTypeConfiguration classes. No data annotations on entities.
- Enums stored as strings (HasConversion<string>()).
- All decimal columns specify precision explicitly (HasPrecision(18,2)).
- Migrations are never edited manually after creation.
- Always use transactions for operations that write to multiple tables.

## API
- Controllers are thin: one line calling mediator.Send(), return mapped HTTP result.
- All list endpoints return PagedResult<T> — no unbounded queries ever.
- All endpoints require [Authorize] unless explicitly marked [AllowAnonymous].
- Role constants live in a single static class (Roles.Admin, Roles.Manager, Roles.LoanOfficer).

## Security
- Passwords hashed via ASP.NET Core Identity. Never store plain text or roll your own hashing.
- JWT secret loaded from environment variables only. Never hardcoded or in appsettings.json.
- Connection strings from environment variables only.
- Sensitive fields (password, token) never included in any response DTO.

## Error Handling
- One global exception middleware catches all unhandled exceptions.
- Unhandled exceptions log full stack trace via Serilog and return generic 500 — no stack traces in responses.
- Business failures return Result.Failure() — never throw for expected conditions.

## Testing
- Every handler has at least one test: happy path + one failure case.
- Use Testcontainers for integration tests — no mocking the database.
- No logic that cannot be unit tested. If it's untestable, the design is wrong.

## General
- Async all the way — no .Result or .Wait() anywhere.
- No magic strings — use constants or enums.
- Never return domain entities from handlers — always map to a DTO.
- Fix warnings before marking any task complete. Zero warning tolerance on build.