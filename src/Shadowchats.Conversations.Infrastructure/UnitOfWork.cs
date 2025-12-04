using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Infrastructure.ApplicationDbContext;
using Shadowchats.Conversations.Infrastructure.Enums;

namespace Shadowchats.Conversations.Infrastructure;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    public UnitOfWork(UnitOfWorkPolicy policy, IDbContextFactory<ReadOnlyApplicationDbContext> readOnlyFactory,
        IDbContextFactory<ReadWriteApplicationDbContext> readWriteFactory)
    {
        _policy = policy;
        _readOnlyFactory = readOnlyFactory;
        _readWriteFactory = readWriteFactory;
        _dbContext = null;
        _transaction = null;
        _isDisposed = false;
    }

    public async Task Begin(Type requestType, CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, nameof(UnitOfWork));
        
        if (_dbContext is not null)
            throw new BugException("Is already begun.");

        var dbContextType = _policy.GetDbContextTypeMap(requestType);
        _dbContext = dbContextType switch
        {
            ApplicationDbContextType.ReadOnly => await _readOnlyFactory.CreateDbContextAsync(cancellationToken),
            ApplicationDbContextType.ReadWrite => await _readWriteFactory.CreateDbContextAsync(cancellationToken),
            _ => throw new BugException($"The ApplicationDbContextType {dbContextType} is not supported.")
        };

        if (_policy.RequiresTransaction(requestType))
            _transaction = await _dbContext.BeginTransaction(_policy.GetIsolationLevel(requestType), cancellationToken);
        else
            _transaction = null;
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, nameof(UnitOfWork));
        
        if (_dbContext is null)
            throw new BugException("Is not yet begun or is already ended.");

        if (_transaction is not null)
            await _transaction.CommitAsync(cancellationToken);
    }

    public async Task Rollback(CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, nameof(UnitOfWork));
        
        if (_dbContext is null)
            throw new BugException("Is not yet begun or is already ended.");

        if (_transaction is not null)
            await _transaction.RollbackAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
            return;

        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        if (_dbContext is not null)
        {
            await _dbContext.DisposeAsync();
            _dbContext = null;
        }
        
        _isDisposed = true;
    }

    public BaseApplicationDbContext DbContext
    {
        get
        {
            ObjectDisposedException.ThrowIf(_isDisposed, nameof(UnitOfWork));
            
            return _dbContext ?? throw new BugException("Is not yet begun or is already ended.");
        }
    }
    
    private readonly UnitOfWorkPolicy _policy;

    private readonly IDbContextFactory<ReadOnlyApplicationDbContext> _readOnlyFactory;

    private readonly IDbContextFactory<ReadWriteApplicationDbContext> _readWriteFactory;

    private BaseApplicationDbContext? _dbContext;

    private IDbContextTransaction? _transaction;

    private bool _isDisposed;
}