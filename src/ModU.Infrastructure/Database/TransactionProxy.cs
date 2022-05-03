using Microsoft.EntityFrameworkCore.Storage;
using ModU.Abstract.Database;

namespace ModU.Infrastructure.Database;

internal sealed class TransactionProxy : ITransaction
{
    private readonly IDbContextTransaction _transaction;

    public TransactionProxy(IDbContextTransaction transaction) => _transaction = transaction;
    
    public ValueTask DisposeAsync() => _transaction.DisposeAsync();

    public Task CommitAsync(CancellationToken cancellationToken = new()) =>
        _transaction.CommitAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = new()) =>
        _transaction.RollbackAsync(cancellationToken);
}