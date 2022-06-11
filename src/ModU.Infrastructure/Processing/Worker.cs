using ModU.Abstract.Processing;

namespace ModU.Infrastructure.Processing;

class Worker : IWorker
{
    private Task? _currentJob;

    public bool HasFinished => _currentJob is null || _currentJob.IsCompleted;
    
    public void StartWork<TJob>(TJob job, CancellationToken cancellationToken) where TJob : IJob
    {
        if (!HasFinished)
        {
            throw new InvalidOperationException("A current job did not finish.");
        }

        _currentJob = job.RunAsync(cancellationToken);
    }

    public void StartWork<TJob, TParam>(TJob job, TParam param, CancellationToken cancellationToken) where TJob : IJob<TParam>
    {
        if (!HasFinished)
        {
            throw new InvalidOperationException("A current job did not finish.");
        }

        _currentJob = job.RunAsync(param, cancellationToken);
    }
}