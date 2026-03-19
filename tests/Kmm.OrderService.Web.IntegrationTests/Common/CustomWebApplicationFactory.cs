namespace Kmm.OrderService.Web.IntegrationTests.Common;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddDatabaseInMemory();
            services.AddOrderServiceMock();
        });
    }
}

