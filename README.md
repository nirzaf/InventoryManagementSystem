# Inventory Management System

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10-purple.svg)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue.svg)](https://www.postgresql.org/)

A modern, open-source ASP.NET Core MVC web application for managing inventory, stock operations, purchase orders, suppliers, and locations. Built with Clean Architecture for maintainability and testability.

## Technology Stack

| Component | Technology |
|-----------|-----------|
| Framework | .NET 10 / ASP.NET Core MVC |
| Database | PostgreSQL 16 |
| ORM | Entity Framework Core 10 (with Npgsql) |
| CQRS / Mediator | MediatR 12 |
| UI Components | MudBlazor 9 |
| Authentication | ASP.NET Core Identity (RBAC) |
| Logging | Serilog (ASP.NET Core integration) |
| PDF Reports | QuestPDF |
| Validation | FluentValidation |
| Object Mapping | AutoMapper |
| API Versioning | Asp.Versioning.Mvc |
| Testing | xUnit, Moq, FluentAssertions, AutoFixture |
| Integration Testing | Microsoft.AspNetCore.Mvc.Testing, EF Core InMemory |
| Containerization | Docker, Docker Compose |
| Web Server (runtime) | Kestrel via ASP.NET Core 10 |

## Prerequisites

- .NET 10 SDK or later
- PostgreSQL 16+ (or use the bundled Docker Compose setup)
- Any editor: Visual Studio 2022, VS Code, or JetBrains Rider

## Getting Started

### 1. Install PostgreSQL

**Using Homebrew (macOS):**
```bash
brew install postgresql
brew services start postgresql
createdb InventoryDB
```

**Using Docker:**
```bash
docker run --name inventory-postgres -e POSTGRES_PASSWORD=yourpassword -p 5432:5432 -d postgres:16
```

**Or use the bundled Docker Compose stack (app + database):**
```bash
docker compose up -d
```

### 2. Configure Database Connection

Copy the development settings template and customize it:

```bash
# Copy development template (not committed to git)
cp InventoryManagementSystem.Web/appsettings.Development.json.example InventoryManagementSystem.Web/appsettings.Development.json
```

Edit `appsettings.Development.json` with your PostgreSQL credentials.

Or set an environment variable directly:

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Database=InventoryDB;Username=postgres;Password=yourpassword"
```

### 3. Run Database Migrations

```bash
# Install EF Core tools if not installed
dotnet tool install --global dotnet-ef

# Add initial migration
dotnet ef migrations add InitialCreate \
  --project InventoryManagementSystem.Infrastructure \
  --startup-project InventoryManagementSystem.Web

# Update database
dotnet ef database update \
  --project InventoryManagementSystem.Infrastructure \
  --startup-project InventoryManagementSystem.Web
```

### 4. Run the Application

```bash
cd InventoryManagementSystem.Web
dotnet run
```

The application will be available at: `https://localhost:5001`

### 5. Default Login

(Only available in Development mode when configured in `appsettings.Development.json`)

Configure your admin credentials in `appsettings.Development.json` under `AdminSettings`.

## Project Structure

```
InventoryManagementSystem/
├── InventoryManagementSystem.Core/         # Domain layer (entities, interfaces, services)
├── InventoryManagementSystem.Infrastructure/ # Data access (DbContext, repositories)
├── InventoryManagementSystem.Web/          # Web application (controllers, views)
├── InventoryManagementSystem.Tests/        # Unit and integration tests
└── IMPLEMENTATION_GUIDE.md                 # Detailed implementation guide
```

## Features

- Clean Architecture with separation of concerns
- Domain entities for inventory management (Items, Stock, Locations, Suppliers, Purchase Orders)
- Stock operations: Receive, Transfer, Sell with transaction history
- Purchase Order management with line items and status tracking
- Entity Framework Core with PostgreSQL support
- Repository pattern for data access
- ASP.NET Core Identity with role-based access control (Admin, Manager, Staff)
- MudBlazor UI components for responsive design
- Serilog structured logging (console + rolling file)
- QuestPDF for report generation
- MediatR-based CQRS pipeline for application logic
- ASP.NET API versioning for headless/API surfaces
- Async/await patterns throughout
- Solution structure ready for extension and contribution

## Architecture

### Clean Architecture Layers

1. **Core Layer** - Domain entities, interfaces, business logic
2. **Infrastructure Layer** - Data access, external services
3. **Web Layer** - MVC controllers, Razor views, UI
4. **Tests Layer** - Unit and integration tests

### Design Patterns

- Repository Pattern
- Dependency Injection
- Unit of Work (via EF Core DbContext)
- Service Layer Pattern
- CQRS with MediatR
- API Versioning

## Development

### Add New Migration

```bash
dotnet ef migrations add MigrationName \
  --project InventoryManagementSystem.Infrastructure \
  --startup-project InventoryManagementSystem.Web
```

### Update Database

```bash
dotnet ef database update \
  --project InventoryManagementSystem.Infrastructure \
  --startup-project InventoryManagementSystem.Web
```

### Run Tests

```bash
dotnet test
```

### Build Solution

```bash
dotnet build
```

## Production Deployment

### Environment Variables

Set these environment variables in production (never commit secrets):

- `ConnectionStrings__DefaultConnection` — PostgreSQL connection string
- `ASPNETCORE_ENVIRONMENT` — `Production`
- `ASPNETCORE_URLS` — e.g. `http://0.0.0.0:8080`

### Publish Application

```bash
dotnet publish -c Release -o ./publish
```

### Docker

A multi-stage Dockerfile (targeting `mcr.microsoft.com/dotnet/sdk:10.0` and `mcr.microsoft.com/dotnet/aspnet:10.0`) and a `docker-compose.yml` are provided.

```bash
docker compose up --build -d
```

The app container listens on port `8080` and connects to the bundled `postgres:16-alpine` service.

## Testing

### Unit Tests

```bash
cd InventoryManagementSystem.Tests
dotnet test --filter "Category=Unit"
```

### Integration Tests

```bash
dotnet test --filter "Category=Integration"
```

## Contributing

We welcome contributions! Please see:

- [CONTRIBUTING.md](CONTRIBUTING.md) for development workflow and guidelines
- [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md) for community standards
- [SECURITY.md](SECURITY.md) for reporting vulnerabilities

## License

This project is licensed under the [MIT License](LICENSE). You are free to use, modify, distribute, and sell this software.

## Support

- [GitHub Issues](https://github.com/YOUR_ORG/InventoryManagementSystem/issues) for bugs and features
- [GitHub Discussions](https://github.com/YOUR_ORG/InventoryManagementSystem/discussions) for questions

## Acknowledgements

This project stands on the shoulders of open source. We gratefully acknowledge the following projects and their contributors:

### Runtime & Frameworks
- [.NET](https://github.com/dotnet/core) and [ASP.NET Core](https://github.com/dotnet/aspnetcore) — Copyright (c) .NET Foundation and contributors. Licensed under the [MIT License](https://github.com/dotnet/aspnetcore/blob/main/LICENSE.txt).
- [Kestrel web server](https://github.com/dotnet/aspnetcore) — part of the ASP.NET Core project, MIT License.

### Database & Data Access
- [PostgreSQL](https://www.postgresql.org/) — Copyright (c) The PostgreSQL Global Development Group. Licensed under the [PostgreSQL License](https://www.postgresql.org/about/licence/) (a permissive BSD-style license).
- [Entity Framework Core](https://github.com/dotnet/efcore) — Copyright (c) .NET Foundation and contributors. Licensed under the [MIT License](https://github.com/dotnet/efcore/blob/main/LICENSE.txt).
- [Npgsql](https://github.com/npgsql/npgsql) — Copyright (c) Npgsql and contributors. Licensed under the [PostgreSQL License](https://github.com/npgsql/npgsql/blob/main/LICENSE).

### Application & Architecture
- [MediatR](https://github.com/LuckyPennySoftware/MediatR) — Copyright (c) Jimmy Bogard and contributors. Licensed under the [Apache License 2.0](https://github.com/LuckyPennySoftware/MediatR/blob/master/LICENSE).
- [AutoMapper](https://github.com/AutoMapper/AutoMapper) — Copyright (c) Jimmy Bogard and contributors. Licensed under the [MIT License](https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt).
- [FluentValidation](https://github.com/FluentValidation/FluentValidation) — Copyright (c) Jeremy Skinner and contributors. Licensed under the [Apache License 2.0](https://github.com/FluentValidation/FluentValidation/blob/main/LICENSE).
- [ASP.NET API Versioning](https://github.com/dotnet/aspnet-api-versioning) — Copyright (c) .NET Foundation and contributors. Licensed under the [MIT License](https://github.com/dotnet/aspnet-api-versioning/blob/master/LICENSE.txt).

### UI
- [MudBlazor](https://github.com/MudBlazor/MudBlazor) — Copyright (c) MudBlazor and contributors. Licensed under the [MIT License](https://github.com/MudBlazor/MudBlazor/blob/master/LICENSE).
- [Razor](https://github.com/dotnet/aspnetcore) — part of the ASP.NET Core project, MIT License.

### Authentication & Security
- [ASP.NET Core Identity](https://github.com/dotnet/aspnetcore) — part of the ASP.NET Core project, MIT License.

### Logging
- [Serilog](https://github.com/serilog/serilog) — Copyright (c) Serilog Contributors. Licensed under the [Apache License 2.0](https://github.com/serilog/serilog/blob/dev/LICENSE).
- [Serilog.AspNetCore](https://github.com/serilog/serilog-aspnetcore) — Copyright (c) Serilog Contributors. Licensed under the [Apache License 2.0](https://github.com/serilog/serilog-aspnetcore/blob/main/LICENSE).

### PDF Generation
- [QuestPDF](https://github.com/QuestPDF/QuestPDF) — Copyright (c) QuestPDF and contributors. Licensed under the [MIT License](https://github.com/QuestPDF/QuestPDF/blob/main/LICENSE).

### Testing
- [xUnit](https://github.com/xunit/xunit) — Copyright (c) .NET Foundation and contributors. Licensed under the [Apache License 2.0](https://github.com/xunit/xunit/blob/main/LICENSE).
- [Moq](https://github.com/devlooped/moq) — Copyright (c) Daniel Cazzulino and contributors. Licensed under the [BSD 3-Clause License](https://github.com/devlooped/moq/blob/main/LICENSE.txt).
- [FluentAssertions](https://github.com/fluentassertions/fluentassertions) — Copyright (c) Dennis Doomen and contributors. Licensed under the [Apache License 2.0](https://github.com/fluentassertions/fluentassertions/blob/develop/LICENSE).
- [AutoFixture](https://github.com/AutoFixture/AutoFixture) — Copyright (c) AutoFixture and contributors. Licensed under the [MIT License](https://github.com/AutoFixture/AutoFixture/blob/master/LICENSE).
- [coverlet.collector](https://github.com/coverlet-coverage/coverlet) — Copyright (c) Toni Solarin-Sodara and contributors. Licensed under the [MIT License](https://github.com/coverlet-coverage/coverlet/blob/master/LICENSE).
- [Microsoft.NET.Test.Sdk](https://github.com/microsoft/vstest) — Copyright (c) Microsoft and contributors. Licensed under the [MIT License](https://github.com/microsoft/vstest/blob/main/LICENSE).

### Tooling & Infrastructure
- [Docker](https://www.docker.com/) and the [mcr.microsoft.com/dotnet](https://hub.docker.com/_/microsoft-dotnet) base images — Copyright (c) Microsoft and Docker, Inc. Used under their respective licenses.
- [Visual Studio](https://visualstudio.microsoft.com/), [Visual Studio Code](https://github.com/microsoft/vscode), and [JetBrains Rider](https://www.jetbrains.com/rider/) — used as development environments.

Special thanks to the open source community — your work makes projects like this possible. If a project used here is not listed, please open an issue or pull request so we can add proper attribution.
