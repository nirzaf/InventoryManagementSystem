#!/usr/bin/env bash
set -euo pipefail

echo "=== Running tests ==="
dotnet test --verbosity normal "$@"

echo "=== All tests pass ==="
