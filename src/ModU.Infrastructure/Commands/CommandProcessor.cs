using ModU.Abstract.Commands;

namespace ModU.Infrastructure.Commands;

internal sealed class CommandProcessor : ICommandProcessor
{
    private readonly IServiceProvider _serviceProvider;

    public CommandProcessor(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Task ProcessAsync<TCommand>(TCommand command, CancellationToken cancellationToken = new()) where TCommand : ICommand
    {
        if (_serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) is not ICommandHandler<TCommand> handler)
        {
            throw new InvalidOperationException($"Handler for command of type '{typeof(TCommand)}' was not registered.");
        }

        return handler.HandleAsync(command, cancellationToken);
    }
}