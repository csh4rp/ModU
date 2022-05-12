using Microsoft.EntityFrameworkCore;
using ModU.Abstract.Database;

namespace ModU.Infrastructure.Database;

public abstract class BaseDbContext : DbContext, IDbContext
{
    public IQueryable<TEntity> Table<TEntity>() where TEntity : class => Set<TEntity>();
    public void AddEntity<TEntity>(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void UpdateEntity<TEntity>(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void RemoveEntity<TEntity>(TEntity entity)
    {
        throw new NotImplementedException();
    }
    
}