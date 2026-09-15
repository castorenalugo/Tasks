# AGENTS.md

## Project Structure
- **Root**: .NET 10 Web API project using ASP.NET Core
- **Main Application**: `Tasks.Api`
- **Tests**: `Tasks.Tests`

## Key Commands
- **Run API**: `dotnet run --project Tasks.Api`
- **Run Tests**: `dotnet test`
- **Build Solution**: `dotnet build`

## Framework Details
- Framework: .NET 10 / C# 13
- Uses ASP.NET Core with Entity Framework Core
- PostgreSQL database (Npgsql provider)
- Uses MSTest for unit and integration testing
- Testcontainers (`postgres:18-alpine`) for integration testing with Postgres

## Code Style & Conventions

- **Primary Constructors:** Always use primary constructors for classes and record declarations.
- **Dependency Fields:** Private fields representing injected dependencies must start with an underscore (e.g., `_logger`, `_service`).
- **Async Method Naming:** Do not append the `Async` suffix to asynchronous method names.
- **Cancellation Tokens:** Always accept a `CancellationToken` parameter in async methods and forward it through to nested async calls.
- **Namespaces:** Always use file-scoped namespace declarations (e.g., `namespace MyProject.Services;`).
- **DTO Properties:** Use the `required` modifier on mandatory properties in DTOs:
  ```csharp
  public required string Name { get; set; }

## Database & EF Core Rules
- Database context: `TasksDbContext`
- Migrations location: `src/api/Tasks.Api/Database/Migrations`
- Connection string in `appsettings.json`
- **Do NOT** manually edit migration snapshot files unless explicitly asked.
- Prefer LINQ pattern matching over complex raw SQL statements where possible.

## Testing Notes
- Integration tests use `CustomWebApplicationFactory` and inherit from `TestsBase`.
- Tests require Docker desktop / daemon active to run `Testcontainers.PostgreSql`.
- Always verify tests pass (`dotnet test`) after refactoring domain logic or endpoint handlers.

## Agent Context & Safety Rules
- **NEVER** read, inspect, or modify files inside `bin/` or `obj/` directories.
- Do NOT generate verbose, conversational explanations. Output precise file paths, concise summaries, and clean code blocks.