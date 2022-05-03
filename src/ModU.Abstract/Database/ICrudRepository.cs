namespace ModU.Abstract.Database;

public interface ICrudRepository<TEntity, in TId>
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = new());
    
    Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new());

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = new());
    
    Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new());

    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = new());
    
    Task RemoveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new());

    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = new());
    
    Task<TEntity?> GetBySpecificationAsync(QuerySpecification<TEntity> querySpecification, CancellationToken cancellationToken = new());
    
    Task<IReadOnlyList<TEntity>> GetAllBySpecificationAsync(QuerySpecification<TEntity> querySpecification, CancellationToken cancellationToken = new());
}