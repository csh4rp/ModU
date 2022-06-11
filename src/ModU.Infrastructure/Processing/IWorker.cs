using ModU.Abstract.Processing;

namespace ModU.Infrastructure.Processing;

public interface IWorker
{
    bool HasFinished { get; }
    
    void StartWork<TJob>(TJob job, CancellationToken cancellationToken) where TJob : IJob;
    
    void StartWork<TJob, TParam>(TJob job, TParam param, CancellationToken cancellationToken) where TJob : IJob<TParam>;
}