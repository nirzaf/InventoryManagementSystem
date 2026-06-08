# ⚠️ DEPRECATED — Legacy WinForms Application

**This project has been superseded by the ASP.NET Core MVC web application.**

## Migration Status

| Component | Legacy (WinForms) | Modern (.NET 10) |
|-----------|-------------------|-------------------|
| UI Framework | Windows Forms (.NET Framework 4.x) | ASP.NET Core MVC + MudBlazor |
| Database | SQL Server (Entity Framework 6) | PostgreSQL 16 (EF Core 10) |
| Reports | Crystal Reports / RDLC | QuestPDF |
| Auth | None (local only) | ASP.NET Core Identity (RBAC) |
| Architecture | Monolithic code-behind | Clean Architecture + CQRS |
| Deployment | Windows .exe installer | Docker containers (cross-platform) |
| API | None | RESTful API v1 with versioning |
| AI/ML | None | ML.NET demand forecasting + anomaly detection |

## Why Deprecated?

1. **Platform Lock-in**: WinForms only runs on Windows. The modern app runs anywhere (Linux, macOS, Windows).
2. **No Web/Mobile Access**: WinForms is desktop-only. The modern app is browser-based.
3. **Outdated Dependencies**: Entity Framework 6, Crystal Reports, SQL Server.
4. **No Modern Patterns**: No DI, no async/await, no testability.
5. **Security**: No authentication, role-based access, or HTTPS.

## What to Use Instead

The replacement ASP.NET Core MVC application lives at:
```
InventoryManagementSystem.Web/
```

Run with Docker:
```bash
docker compose up -d
```

Or locally:
```bash
cd InventoryManagementSystem.Web
dotnet run
```

## Data Migration

If you need to migrate data from the legacy SQL Server database:

1. Export legacy data to CSV using SQL Server Management Studio
2. Import into PostgreSQL using the modern app's seed data pipeline

## Preservation

This directory is preserved for **reference only** — to understand original business logic during migration. It is NOT compiled or included in the modern solution.

The solution (`.sln`) only contains:
- `InventoryManagementSystem.Web` — ASP.NET Core MVC app
- `InventoryManagementSystem.Core` — Domain layer
- `InventoryManagementSystem.Infrastructure` — Data access
- `InventoryManagementSystem.Tests` — Unit & integration tests

---

*Deprecated: June 2026 | Replaced by ASP.NET Core 10 MVC application*
