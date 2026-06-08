# Inventory Management System

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-14%2B-blue.svg)](https://www.postgresql.org/)

A modern, open-source ASP.NET Core MVC web application for managing inventory, stock operations, purchase orders, suppliers, and locations. Built with Clean Architecture for maintainability and testability.

## Migration Status

This project has been migrated from a legacy Windows Forms application (.NET 4.0) to a modern web application using:

- **.NET 9** (can be upgraded to .NET 10 when available)
- **ASP.NET Core MVC** with Razor views
- **Entity Framework Core 9** with PostgreSQL
- **MudBlazor** UI components
- **QuestPDF** for report generation
- **Serilog** for logging
- **ASP.NET Core Identity** for authentication and authorization

## Technology Stack

| Component | Technology |
|-----------|-----------|
| Framework | .NET 9 / ASP.NET Core MVC |
| Database | PostgreSQL 14+ |
| ORM | Entity Framework Core 9 |
| UI Components | MudBlazor |
| Authentication | ASP.NET Core Identity (RBAC) |
| Logging | Serilog |
| PDF Reports | QuestPDF |
| Validation | FluentValidation |
| Object Mapping | AutoMapper |
| Testing | xUnit, Moq, FluentAssertions |

## Prerequisites

- .NET 9 SDK or later
- PostgreSQL 14+
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

## Technology Stack

| Component | Technology |
|-----------|-----------|
| Framework | .NET 9 / ASP.NET Core MVC |
| Database | PostgreSQL 14+ |
| ORM | Entity Framework Core 9 |
| UI Components | MudBlazor |
| Authentication | ASP.NET Core Identity |
| Logging | Serilog |
| PDF Reports | QuestPDF |
| Validation | FluentValidation |
| Object Mapping | AutoMapper |
| Testing | xUnit, Moq, FluentAssertions |

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

### Docker (Optional)

```bash
docker run -d \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Database=InventoryDB;Username=postgres;Password=yourpassword" \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -p 8080:8080 \
  inventory-management-system
```

## Data Migration from SQL Server

To migrate data from the old SQL Server database:

1. Export data from SQL Server to CSV files
2. Create a data import utility (see IMPLEMENTATION_GUIDE.md)
3. Import CSV data into PostgreSQL
4. Verify data integrity

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

## Migration from Windows Forms

This project replaces the legacy Windows Forms application with a modern web-based solution. Key improvements:

- Cross-platform compatibility
- Modern responsive UI
- Better security with role-based access
- Production-ready logging and error handling
- Testable architecture
- Easier deployment and maintenance

See `IMPLEMENTATION_GUIDE.md` for complete migration details.
