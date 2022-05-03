namespace ModU.Abstract.Database;

public interface IUnitOfWork<TDbContext> where TDbContext : IDbContext
{
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = new());
}