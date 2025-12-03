using Microsoft.EntityFrameworkCore;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Infrastructure.ApplicationDbContext;
using Shadowchats.Conversations.Infrastructure.Interfaces;

namespace Shadowchats.Conversations.Infrastructure;

public class UnitOfWork : IInfrastructureUnitOfWork
{
    public UnitOfWork(IDbContextFactory<ReadOnlyApplicationDbContext> readOnlyFactory,
        IDbContextFactory<ReadWriteApplicationDbContext> readWriteFactory)
    {
        _readOnlyFactory = readOnlyFactory;
        _readWriteFactory = readWriteFactory;
        _dbContext = null;
    }
    
    public async Task Begin(TransactionMode mode)
    {
        _activeContext = mode switch
        {
            TransactionMode.ReadWrite => _rw,
            TransactionMode.ReadOnly  => _ro,
            _ => null
        };

        if (_activeContext != null)
            _tx = await _activeContext.Database.BeginTransactionAsync();
    }

    public async Task Commit()
    {
        if (_tx != null)
            await _tx.CommitAsync();
    }

    public async Task Rollback()
    {
        if (_tx != null)
            await _tx.RollbackAsync();
    }
    
    public async Task Begin(IAuthenticationDbContext dbContext, IUnitOfWork.TransactionMode transactionMode)
    {
        if (_dbContext is not null)
            throw new BugException("Is already begun.");
        
        _dbContext = dbContext;

        _transaction = transactionMode switch
        {
            IUnitOfWork.TransactionMode.None => null,
            IUnitOfWork.TransactionMode.WithReadCommitted => await _dbContext.BeginTransaction(),
            _ => throw new BugException("Transaction mode is not supported.")
        };
    }

    public IApplicationDbContext DbContext =>
        _dbContext ?? throw new BugException("Is not yet begun or is already ended.");

    private readonly IDbContextFactory<ReadOnlyApplicationDbContext> _readOnlyFactory;

    private readonly IDbContextFactory<ReadWriteApplicationDbContext> _readWriteFactory;

    private IApplicationDbContext? _dbContext;
}