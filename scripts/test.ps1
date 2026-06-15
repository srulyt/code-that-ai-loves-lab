# Runs the test suite.
# NOTE: tests only exist on the 'baseline-with-tests' branch and later.
# Usage: ./scripts/test.ps1
$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot -Parent
$testProj = Join-Path $root "tests/BackOffice.Tests/BackOffice.Tests.csproj"
if (-not (Test-Path $testProj)) {
    Write-Host "No test project found on this branch." -ForegroundColor Yellow
    Write-Host "Switch to 'baseline-with-tests' (or later) to run tests:" -ForegroundColor Yellow
    Write-Host "    git checkout baseline-with-tests" -ForegroundColor Yellow
    exit 1
}
dotnet test $testProj
