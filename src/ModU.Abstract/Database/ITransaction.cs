namespace ModU.Abstract.Database;

public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = new());

    Task RollbackAsync(CancellationToken cancellationToken = new());
}