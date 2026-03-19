$project = "$PSScriptRoot/../../src/Kmm.OrderService.Infrastructure"
$startupProject = "$PSScriptRoot/../../src/Kmm.OrderService.Web"
$context = "ApplicationDbContext"

dotnet ef database update `
    --project $project `
    --startup-project $startupProject `
    --context $context