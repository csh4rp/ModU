namespace ModU.Abstract.Processing;

public interface IJob
{
    Task RunAsync(CancellationToken cancellationToken = new());
}

public interface IJob<in T>
{
    Task RunAsync(T parameter, CancellationToken cancellationToken = new());
}