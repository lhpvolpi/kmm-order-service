param(
    [Parameter(Mandatory = $true)]
    [string]$MigrationName
)

$project = "$PSScriptRoot/../../src/Kmm.OrderService.Infrastructure"
$startupProject = "$PSScriptRoot/../../src/Kmm.OrderService.Web"
$context = "ApplicationDbContext"

dotnet ef migrations add $MigrationName `
    --project $project `
    --startup-project $startupProject `
    --context $context