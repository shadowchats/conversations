using System.Data;
using Microsoft.EntityFrameworkCore;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Application.UseCases.Test1;
using Shadowchats.Conversations.Application.UseCases.Test2;
using Shadowchats.Conversations.Application.UseCases.Test3;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Infrastructure;

public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private enum State : byte
    {
        NotStarted = 0,
        Begun = 1,
        Completed = 2,
    }
    
    public UnitOfWork(IDbContextFactory<ReadOnlyApplicationDbContext> readOnlyFactory,
        IDbContextFactory<ReadWriteApplicationDbContext> readWriteFactory)
    {
        _readOnlyFactory = readOnlyFactory;
        _readWriteFactory = readWriteFactory;
        _state = State.NotStarted;
        _dbContext = null;
        _isDisposed = false;
    }

    public async Task Begin(Type requestType, CancellationToken cancellationToken)
    {
        EnsureNotDisposed();
    
        if (_state != State.NotStarted)
            throw new BugException();

        BaseApplicationDbContext? dbContext = null;
        try
        {
            var dbContextType = UnitOfWorkPolicy.GetDbContextType(requestType);
            dbContext = dbContextType switch
            {
                ApplicationDbContextType.ReadOnly => 
                    await _readOnlyFactory.CreateDbContextAsync(cancellationToken),
                ApplicationDbContextType.ReadWrite => 
                    await _readWriteFactory.CreateDbContextAsync(cancellationToken),
                _ => throw new BugException()
            };

            if (UnitOfWorkPolicy.RequiresTransaction(requestType))
                await dbContext.Database.BeginTransactionAsync(
                    UnitOfWorkPolicy.GetIsolationLevel(requestType), cancellationToken);
        
            _dbContext = dbContext;
            _state = State.Begun;
        }
        catch
        {
            if (dbContext is not null)
                await dbContext.DisposeAsync();
            
            throw;
        }
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        EnsureNotDisposed();
        
        if (_state != State.Begun)
            throw new BugException();

        if (_dbContext!.Database.CurrentTransaction is not null)
            await _dbContext.Database.CommitTransactionAsync(cancellationToken);
        
        _state = State.Completed;
    }

    public async Task Rollback(CancellationToken cancellationToken)
    {
        EnsureNotDisposed();
        
        if (_state != State.Begun)
            throw new BugException();

        if (_dbContext!.Database.CurrentTransaction is not null)
            await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
        
        _state = State.Completed;
    }
    
    private void EnsureNotDisposed() => ObjectDisposedException.ThrowIf(_isDisposed, nameof(UnitOfWork));

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;

        if (_dbContext is not null) 
            await _dbContext.DisposeAsync();
    }

    public BaseApplicationDbContext DbContext
    {
        get
        {
            EnsureNotDisposed();
            
            return _state != State.Begun ? throw new BugException() : _dbContext!;
        }
    }

    private readonly IDbContextFactory<ReadOnlyApplicationDbContext> _readOnlyFactory;

    private readonly IDbContextFactory<ReadWriteApplicationDbContext> _readWriteFactory;

    private State _state;
    
    private BaseApplicationDbContext? _dbContext;

    private bool _isDisposed;
}

file static class UnitOfWorkPolicy
{
    public static bool RequiresTransaction(Type requestType) => !RequiresTransactionMap.TryGetValue(requestType, out var value) ? throw new BugException() : value;

    public static IsolationLevel GetIsolationLevel(Type requestType) => !IsolationLevelMap.TryGetValue(requestType, out var value) ? throw new BugException() : value;

    public static ApplicationDbContextType GetDbContextType(Type requestType) => !DbContextTypeMap.TryGetValue(requestType, out var value) ? throw new BugException() : value;

    private static readonly Dictionary<Type, bool> RequiresTransactionMap = new()
    {
        { typeof(Test1Command), true },
        { typeof(Test2Query), false },
        { typeof(Test3Command), true }
    };

    private static readonly Dictionary<Type, IsolationLevel> IsolationLevelMap = new()
    {
        { typeof(Test1Command), IsolationLevel.ReadCommitted },
        { typeof(Test3Command), IsolationLevel.ReadCommitted }
    };

    private static readonly Dictionary<Type, ApplicationDbContextType> DbContextTypeMap = new()
    {
        { typeof(Test1Command), ApplicationDbContextType.ReadWrite },
        { typeof(Test2Query), ApplicationDbContextType.ReadOnly },
        { typeof(Test3Command), ApplicationDbContextType.ReadWrite }
    };
}

file enum ApplicationDbContextType : byte
{
    ReadOnly = 0,
    ReadWrite = 1
}