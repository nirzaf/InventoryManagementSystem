#!/usr/bin/env bash
set -euo pipefail

# === Inventory Management System — Production Deployment ===
# Usage: ./scripts/deploy.sh [--build] [--migrate]
# Prerequisites: Docker + Docker Compose installed on target host

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"

cd "$PROJECT_DIR"

# Parse flags
DO_BUILD=false
DO_MIGRATE=false
for arg in "$@"; do
    case "$arg" in
        --build) DO_BUILD=true ;;
        --migrate) DO_MIGRATE=true ;;
        *) echo "Unknown flag: $arg"; exit 1 ;;
    esac
done

echo "=== Inventory Management System — Deployment ==="

# Load .env if present
if [ -f .env ]; then
    set -a
    source .env
    set +a
    echo "✅ Loaded .env configuration"
else
    echo "⚠️  No .env file found — using defaults from docker-compose.yml"
fi

# Pull latest images or build locally
if [ "$DO_BUILD" = true ]; then
    echo "🔨 Building Docker images..."
    docker compose build --no-cache
else
    echo "📥 Pulling Docker images..."
    docker compose pull 2>/dev/null || true
fi

# Start stack
echo "🚀 Starting services..."
docker compose up -d --wait

# Show status
echo ""
echo "📊 Service status:"
docker compose ps

# Run migrations if requested
if [ "$DO_MIGRATE" = true ]; then
    echo ""
    echo "🗄️  Running database migrations..."
    docker compose exec -T app dotnet ef database update --no-build || true
fi

echo ""
echo "✅ Deployment complete!"
echo "   App:  http://localhost:${APP_PORT:-8080}"
echo "   Logs: docker compose logs -f app"
echo ""
echo "   Useful commands:"
echo "     docker compose logs -f app     # Follow app logs"
echo "     docker compose restart app     # Restart the app"
echo "     docker compose down            # Stop everything"
echo "     docker compose down -v         # Stop + remove volumes"
