#!/usr/bin/env bash
set -euo pipefail

echo "=== Restoring packages ==="
dotnet restore

echo "=== Building solution ==="
dotnet build --no-restore --configuration Release

echo "=== Build complete ==="
