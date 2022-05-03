using Microsoft.Extensions.Logging;
using ModU.Abstract.Commands;
using ModU.Abstract.Database;

namespace ModU.Infrastructure.Commands.Decorators;

internal sealed class TransactionalDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionalDecorator<TCommand>> _logger;

    public TransactionalDecorator(ICommandHandler<TCommand> handler, IUnitOfWork unitOfWork, ILogger<TransactionalDecorator<TCommand>> logger)
    {
        _handler = handler;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        _logger.LogTrace("Starting transaction for command: '{CommandType}'.", typeof(TCommand).FullName);
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            await _handler.HandleAsync(command, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _logger.LogTrace("Committed transaction for command: '{CommandType}'.", typeof(TCommand).FullName);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogTrace("Rolled back transaction for command: '{CommandType}'.", typeof(TCommand).FullName);
            throw;
        }
    }
}

internal sealed class TransactionalDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _handler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionalDecorator<TCommand, TResult>> _logger;

    public TransactionalDecorator(ICommandHandler<TCommand, TResult> handler, IUnitOfWork unitOfWork, ILogger<TransactionalDecorator<TCommand, TResult>> logger)
    {
        _handler = handler;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        TResult result;
        _logger.LogTrace("Starting transaction for command: '{CommandType}'.", typeof(TCommand).FullName);
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            result = await _handler.HandleAsync(command, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _logger.LogTrace("Committed transaction for command: '{CommandType}'.", typeof(TCommand).FullName);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogTrace("Rolled back transaction for command: '{CommandType}'.", typeof(TCommand).FullName);
            throw;
        }

        return result;
    }
}