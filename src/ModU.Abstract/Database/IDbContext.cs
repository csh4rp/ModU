namespace ModU.Abstract.Database;

public interface IDbContext
{
    IQueryable<TEntity> Table<TEntity>() where  TEntity : class;

    void AddEntity<TEntity>(TEntity entity);
    
    void UpdateEntity<TEntity>(TEntity entity);

    void RemoveEntity<TEntity>(TEntity entity);
}