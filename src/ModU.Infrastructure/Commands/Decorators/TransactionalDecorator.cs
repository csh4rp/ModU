using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Commands;

namespace ModU.Infrastructure.Commands.Decorators;

internal sealed class TransactionalDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly UnitOfWorTypeRegistry _unitOfWorTypeRegistry;
    private readonly IServiceProvider _serviceProvider;

    public TransactionalDecorator(ICommandHandler<TCommand> handler, UnitOfWorTypeRegistry unitOfWorTypeRegistry, IServiceProvider serviceProvider)
    {
        _handler = handler;
        _unitOfWorTypeRegistry = unitOfWorTypeRegistry;
        _serviceProvider = serviceProvider;
    }

    public Task HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        if (!_unitOfWorTypeRegistry.TryGetContextType(typeof(ICommandHandler<TCommand>), out var unitOfWorkType))
        {
            throw new InvalidOperationException(
                $"Unit of work is not registered for type: '{typeof(ICommandHandler<TCommand>)}'.");
        }

        var unitOffWork = (IUnitOffWork) _serviceProvider.GetRequiredService(unitOfWorkType!);
        return unitOffWork.ExecuteAsync(command, _handler, cancellationToken);
    }
}

internal sealed class TransactionalDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _handler;
    private readonly UnitOfWorTypeRegistry _unitOfWorTypeRegistry;
    private readonly IServiceProvider _serviceProvider;

    public TransactionalDecorator(ICommandHandler<TCommand, TResult> handler, UnitOfWorTypeRegistry unitOfWorTypeRegistry, IServiceProvider serviceProvider)
    {
        _handler = handler;
        _unitOfWorTypeRegistry = unitOfWorTypeRegistry;
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        if (!_unitOfWorTypeRegistry.TryGetContextType(typeof(ICommandHandler<TCommand, TResult>), out var unitOfWorkType))
        {
            throw new InvalidOperationException(
                $"Unit of work is not registered for type: '{typeof(ICommandHandler<TCommand, TResult>)}'.");
        }

        var unitOffWork = (IUnitOffWork) _serviceProvider.GetRequiredService(unitOfWorkType!);
        return unitOffWork.ExecuteAsync(command, _handler, cancellationToken);
    }
}