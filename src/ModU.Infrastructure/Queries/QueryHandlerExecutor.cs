using ModU.Abstract.Queries;

namespace ModU.Infrastructure.Queries;

internal sealed class QueryHandlerExecutor<TQuery, TResult> : BaseQueryHandlerExecutor<TResult> where TQuery : IQuery<TResult>
{
    
        public override Task<TResult> ExecuteHandlerAsync(object handler, object query, CancellationToken cancellationToken)
        {
            var handlerToExecute = (IQueryHandler<TQuery, TResult>)handler;
            var commandToExecute = (TQuery)query;
            return handlerToExecute.HandleAsync(commandToExecute, cancellationToken);
        }
        
}