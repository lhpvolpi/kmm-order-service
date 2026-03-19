$project = "$PSScriptRoot/../../src/Kmm.OrderService.Infrastructure"
$startupProject = "$PSScriptRoot/../../src/Kmm.OrderService.Web"
$context = "ApplicationDbContext"
$output = "$PSScriptRoot/script.sql"

dotnet ef migrations script `
    --project $project `
    --startup-project $startupProject `
    --context $context `
    --output $output `
    --idempotent