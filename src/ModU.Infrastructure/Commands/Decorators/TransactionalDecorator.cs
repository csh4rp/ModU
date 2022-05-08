using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Commands;

namespace ModU.Infrastructure.Commands.Decorators;

internal sealed class TransactionalDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly UnitOfWorkTypeRegistry _unitOfWorkTypeRegistry;
    private readonly IServiceProvider _serviceProvider;

    public TransactionalDecorator(ICommandHandler<TCommand> handler, UnitOfWorkTypeRegistry unitOfWorkTypeRegistry, IServiceProvider serviceProvider)
    {
        _handler = handler;
        _unitOfWorkTypeRegistry = unitOfWorkTypeRegistry;
        _serviceProvider = serviceProvider;
    }

    public Task HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        if (!_unitOfWorkTypeRegistry.TryGetContextType(typeof(ICommandHandler<TCommand>), out var unitOfWorkType))
        {
            throw new InvalidOperationException(
                $"Unit of work is not registered for type: '{typeof(ICommandHandler<TCommand>)}'.");
        }

        var unitOffWork = (IUnitOffWork) _serviceProvider.GetRequiredService(unitOfWorkType!);
        return unitOffWork.ExecuteAsync(command, _handler, cancellationToken);
    }
}