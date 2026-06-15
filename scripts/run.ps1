# Runs the BackOffice.Api Minimal API from the repo root.
# Usage: ./scripts/run.ps1
$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot -Parent
Write-Host "Starting BackOffice.Api (http://localhost:5080) ..." -ForegroundColor Cyan
dotnet run --project (Join-Path $root "src/BackOffice.Api/BackOffice.Api.csproj")
