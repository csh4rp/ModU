using Microsoft.EntityFrameworkCore;
using ModU.Abstract.Database;

namespace ModU.Infrastructure.Database;

internal sealed class CrudRepository<TEntity, TId, TDbContext> : ICrudRepository<TEntity, TId> where TEntity : class where TDbContext : DbContext 
{
    private readonly TDbContext _dbContext;
    private readonly DbSet<TEntity> _set;

    public CrudRepository(TDbContext dbContext)
    {
        _dbContext = dbContext;
        _set = dbContext.Set<TEntity>();
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = new())
    {
        _set.Add(entity);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new())
    {
        _set.AddRange(entities);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = new()) => _dbContext.SaveChangesAsync(cancellationToken);

    public Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new()) => _dbContext.SaveChangesAsync(cancellationToken);

    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = new())
    {
        _set.Remove(entity);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task RemoveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new())
    {
        _set.RemoveRange(entities);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = new())
    {
        return await _set.FindAsync(new object?[] { id }, cancellationToken);
    }

    public Task<TEntity?> GetBySpecificationAsync(QuerySpecification<TEntity> querySpecification,
        CancellationToken cancellationToken = new())
    {
        var query = _set.Where(querySpecification.Criteria());
        return querySpecification.Include().Aggregate(query, (queryable, include) => queryable.Include(include))
            .TagWith($"Specification: {querySpecification.GetType().FullName}")
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllBySpecificationAsync(QuerySpecification<TEntity> querySpecification,
        CancellationToken cancellationToken = new())
    {
        var query = _set.Where(querySpecification.Criteria());
        return await querySpecification.Include().Aggregate(query, (queryable, include) => queryable.Include(include))
            .TagWith($"Specification: {querySpecification.GetType().FullName}")
            .ToListAsync(cancellationToken);
    }
}