using Microsoft.EntityFrameworkCore;
using ModU.Abstract.Database;

namespace ModU.Infrastructure.Database;

public abstract class BaseDbContext : DbContext, IDbContext
{
    public IQueryable<TEntity> Table<TEntity>() where TEntity : class => Set<TEntity>();
    
}