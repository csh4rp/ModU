using System.Collections.Concurrent;
using ModU.Abstract.Queries;

namespace ModU.Infrastructure.Queries;

internal sealed class QueryProcessor : IQueryProcessor
{
    private static readonly ConcurrentDictionary<Type, object> Executors = new();
    private readonly IServiceProvider _serviceProvider;

    public QueryProcessor(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = new())
    {
        var queryType = query.GetType();
        var resultType = typeof(TResult);
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, resultType);
        var handler = _serviceProvider.GetService(handlerType);
        if (handler is null)
        {
            throw new InvalidOperationException($"Handler for query of type '{queryType}' was not registered.");
        }
        
        var executorType = typeof(QueryHandlerExecutor<,>).MakeGenericType(queryType, resultType);
        var executor = (BaseQueryHandlerExecutor<TResult>) Executors.GetOrAdd(executorType, 
            t => (BaseQueryHandlerExecutor<TResult>)Activator.CreateInstance(t)!);
        return executor.ExecuteHandlerAsync(handler, query, cancellationToken);
    }
}