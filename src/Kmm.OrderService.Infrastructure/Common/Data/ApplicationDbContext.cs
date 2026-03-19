using Kmm.AspNetCore.FeatureFlag.Providers.EFCore.Data;
using Kmm.OrderService.Application.Common.Entities;
using Kmm.OrderService.Domain.Common.Entities;
using Kmm.OrderService.Domain.Entities;

namespace Kmm.OrderService.Infrastructure.Common.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IUser _user;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUser user) : base(options)
    {
        _user = user;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        ApplyGlobalQueryFilters(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
    {
        ApplyGlobalQueryFilter<Order>(modelBuilder);
        ApplyGlobalQueryFilter<OrderItem>(modelBuilder);
    }

    private void ApplyGlobalQueryFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseAuditableEntity
       => modelBuilder.Entity<TEntity>().HasQueryFilter(i => i.CreatedBy == _user.CustomerId);
}

public sealed class ApplicationDbContextMigrator
{
    private readonly ILogger<ApplicationDbContextMigrator> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextMigrator(
        ILogger<ApplicationDbContextMigrator> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task ApplyMigrationsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_context.Database.CanConnect())
            {
                return;
            }

            await _context.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying database migrations.");
            throw;
        }
    }
}

public sealed class FeatureFlagsDbContextMigrator
{
    private readonly ILogger<FeatureFlagsDbContextMigrator> _logger;
    private readonly FeatureFlagsDbContext _context;

    public FeatureFlagsDbContextMigrator(
        ILogger<FeatureFlagsDbContextMigrator> logger,
        FeatureFlagsDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task ApplyMigrationsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_context.Database.CanConnect())
            {
                return;
            }

            await _context.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying database migrations.");
            throw;
        }
    }
}

public sealed class ProductSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductSeeder> _logger;

    public ProductSeeder(ApplicationDbContext context, ILogger<ProductSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!_context.Database.CanConnect())
            {
                return;
            }

            if (await _context.Products.AnyAsync(cancellationToken))
            {
                _logger.LogInformation("Products already seeded.");
                return;
            }

            var products = new List<Product>
            {
                new Product("Mouse", 49.90m, 20),
                new Product("Teclado", 129.90m, 20),
                new Product("Monitor", 999.90m, 20),
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Seeded {Count} products.", products.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying the product seeder.");
            throw;
        }
    }
}
