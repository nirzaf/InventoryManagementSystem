# === Build Stage ===
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files for layer caching
COPY InventoryManagementSystem.sln .
COPY InventoryManagementSystem.Web/InventoryManagementSystem.Web.csproj InventoryManagementSystem.Web/
COPY InventoryManagementSystem.Core/InventoryManagementSystem.Core.csproj InventoryManagementSystem.Core/
COPY InventoryManagementSystem.Infrastructure/InventoryManagementSystem.Infrastructure.csproj InventoryManagementSystem.Infrastructure/

# Restore dependencies (cached unless csproj changes)
RUN dotnet restore

# Copy remaining source
COPY . .

# Publish the web app
WORKDIR /src/InventoryManagementSystem.Web
RUN dotnet publish -c Release -o /app --no-restore

# === Runtime Stage ===
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create non-root user
RUN adduser --disabled-password --gecos "" appuser && \
    mkdir -p /app/logs && \
    chown -R appuser:appuser /app

USER appuser

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "InventoryManagementSystem.Web.dll"]
