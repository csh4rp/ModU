namespace ModU.Abstract.Database;

public interface IDbContext
{
    IQueryable<TEntity> Table<TEntity>() where  TEntity : class;
}