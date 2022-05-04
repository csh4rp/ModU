using Microsoft.Extensions.Logging;
using ModU.Abstract.Commands;
using ModU.Infrastructure.Database;

namespace ModU.Infrastructure.Commands;

public sealed class UnitOfWork<TDbContext> where TDbContext : BaseDbContext
{
    private readonly TDbContext _dbContext;
    private readonly ILogger<UnitOfWork<TDbContext>> _logger;

    public UnitOfWork(TDbContext dbContext, ILogger<UnitOfWork<TDbContext>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Execute<TCommand>(TCommand command, ICommandHandler<TCommand> commandHandler,
        CancellationToken cancellationToken = new()) where TCommand : ICommand
    {
        _logger.LogInformation("Starting transaction for: '{CommandType}'.", typeof(TCommand));
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await commandHandler.HandleAsync(command, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _logger.LogInformation("Committed transaction for: '{CommandType}'.", typeof(TCommand));
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogInformation("Rolled back transaction for: '{CommandType}'.", typeof(TCommand));
            throw;
        }
    }
}