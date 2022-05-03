using System.Collections.Concurrent;
using ModU.Abstract.Commands;

namespace ModU.Infrastructure.Commands;

internal sealed class CommandProcessor : ICommandProcessor
{
    private static readonly ConcurrentDictionary<Type, object> Executors = new();
    private readonly IServiceProvider _serviceProvider;

    public CommandProcessor(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Task ProcessAsync(ICommand command, CancellationToken cancellationToken = new())
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
        var handler = _serviceProvider.GetService(handlerType);
        if (handler is null)
        {
            throw new InvalidOperationException($"Handler for command of type '{commandType}' was not registered");
        }
        
        var executorType = typeof(CommandHandlerExecutor<>).MakeGenericType(commandType);
        var executor = (BaseCommandHandlerExecutor) Executors.GetOrAdd(executorType,
            t => (BaseCommandHandlerExecutor)Activator.CreateInstance(t)!);
        return executor.ExecuteHandlerAsync(handler, command, cancellationToken);
    }

    public Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = new())
    {
        var commandType = command.GetType();
        var resultType = typeof(TResult);
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, resultType);
        var handler = _serviceProvider.GetService(handlerType);
        if (handler is null)
        {
            throw new InvalidOperationException($"Handler for command of type '{commandType}' was not registered");
        }

        var executorType = typeof(CommandHandlerExecutor<,>).MakeGenericType(commandType, resultType);
        var executor = (BaseCommandHandlerExecutor<TResult>) Executors.GetOrAdd(executorType, 
            t => (BaseCommandHandlerExecutor<TResult>)Activator.CreateInstance(t)!);
        return executor.ExecuteHandlerAsync(handler, command, cancellationToken);
    }
}