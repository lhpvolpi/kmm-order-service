$project = "$PSScriptRoot/../../src/Kmm.OrderService.Infrastructure"
$startupProject = "$PSScriptRoot/../../src/Kmm.OrderService.Web"
$context = "ApplicationDbContext"

dotnet ef migrations remove `
    --project $project `
    --startup-project $startupProject `
    --context $context