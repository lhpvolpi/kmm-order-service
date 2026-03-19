using Kmm.OrderService.Application.Common.Data;

namespace Kmm.OrderService.Infrastructure.Common.Data;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork, IAsyncDisposable
{
    private bool _disposed;
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("Transaction has not been started.");
        }

        await context.SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);

        await DisposeAsync();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }

        await DisposeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

