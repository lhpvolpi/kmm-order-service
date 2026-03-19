#!/bin/bash

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT="$SCRIPT_DIR/../../src/Kmm.OrderService.Infrastructure"
STARTUP_PROJECT="$SCRIPT_DIR/../../src/Kmm.OrderService.Web"
CONTEXT="ApplicationDbContext"

dotnet ef migrations list \
  --project "$PROJECT" \
  --startup-project "$STARTUP_PROJECT" \
  --context "$CONTEXT"