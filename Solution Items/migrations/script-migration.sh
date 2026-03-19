#!/bin/bash

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT="$SCRIPT_DIR/../../src/Kmm.OrderService.Infrastructure"
STARTUP_PROJECT="$SCRIPT_DIR/../../src/Kmm.OrderService.Web"
CONTEXT="ApplicationDbContext"
OUTPUT="$SCRIPT_DIR/script.sql"

dotnet ef migrations script \
  --project "$PROJECT" \
  --startup-project "$STARTUP_PROJECT" \
  --context "$CONTEXT" \
  --output "$OUTPUT" \
  --idempotent