using Kmm.OrderService.Infrastructure.Common.Data;

namespace Kmm.OrderService.Web.IntegrationTests;

public static class DependencyInjection
{
    private static SqliteConnection _connection = null!;

    public static void AddDatabaseInMemory(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(
            i => i.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(_connection));

        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }

    public static void AddOrderServiceMock(this IServiceCollection services)
    {


    }
}

