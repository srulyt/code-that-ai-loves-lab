# Restores the JSON "database" to its seeded state by re-copying the source Data files
# over any runtime-modified copies in bin/.
# Usage: ./scripts/reset-data.ps1
$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot -Parent
$src = Join-Path $root "src/BackOffice.Api/Data"
Write-Host "Source seed data lives in: $src" -ForegroundColor Cyan
Get-ChildItem -Path (Join-Path $root "src/BackOffice.Api/bin") -Recurse -Filter "*.json" -ErrorAction SilentlyContinue |
    Where-Object { $_.FullName -match "[\\/]Data[\\/]" } |
    ForEach-Object {
        $name = $_.Name
        Copy-Item (Join-Path $src $name) $_.FullName -Force
        Write-Host "Reset $($_.FullName)" -ForegroundColor Green
    }
Write-Host "Done. Runtime data reset to seed." -ForegroundColor Cyan
