using System.Linq.Expressions;

namespace ModU.Abstract.Database;

public abstract class QuerySpecification<TEntity>
{
    public abstract Expression<Func<TEntity, bool>> Criteria();

    public virtual IEnumerable<Expression<Func<TEntity, object>>> Include()
    {
        yield break;
    }
}