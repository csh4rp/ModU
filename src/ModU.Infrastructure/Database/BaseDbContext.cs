using Microsoft.EntityFrameworkCore;
using ModU.Abstract.Database;

namespace ModU.Infrastructure.Database;

public abstract class BaseDbContext : DbContext, IDbContext
{
    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = new())
    {
        var transaction = await Database.BeginTransactionAsync(cancellationToken);
        return new TransactionProxy(transaction);
    }

    public IQueryable<TEntity> Table<TEntity>() where TEntity : class => Set<TEntity>();
}