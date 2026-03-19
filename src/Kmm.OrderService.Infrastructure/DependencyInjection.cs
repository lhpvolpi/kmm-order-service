using Kmm.OrderService.Application.Common.Auth;
using Kmm.OrderService.Application.Common.Data;
using Kmm.OrderService.Application.Common.Entities;
using Kmm.OrderService.Application.Common.Repositories;
using Kmm.OrderService.Infrastructure.Common.Auth;
using Kmm.OrderService.Infrastructure.Common.Data;
using Kmm.OrderService.Infrastructure.Common.Identity;
using Kmm.OrderService.Infrastructure.Common.Interceptors;
using Kmm.OrderService.Infrastructure.Repositories;

namespace Kmm.OrderService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // interceptors
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        // DbContext 
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetRequiredService<ISaveChangesInterceptor>());
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<ApplicationDbContextMigrator>();
        services.AddScoped<ProductSeeder>();

        // UoW e repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        // identity
        services.AddScoped<IUser, CurrentUser>();

        // jwt
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
    }

    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var applicationDbContextInitialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextMigrator>();
        await applicationDbContextInitialiser.ApplyMigrationsAsync();
    }

    public static async Task CreateSeedAsync(this WebApplication app, CancellationToken cancellationToken)
    {
        using var scope = app.Services.CreateScope();

        var productSeeder = scope.ServiceProvider.GetRequiredService<ProductSeeder>();
        await productSeeder.SeedAsync(cancellationToken);
    }
}
