using System.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shadowchats.Conversations.Application.Common;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Aggregates;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Infrastructure;

public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private enum State : byte
    {
        NotStarted = 0,
        Begun = 1
    }
    
    private enum ApplicationDbContextType : byte
    {
        ReadOnly = 0,
        ReadWrite = 1
    }
    
    private sealed record Options
    {
        public static Options Map(UnitOfWorkOptions applicationOptions)
        {
            var dbContextType = applicationOptions.DataAccessMode switch
            {
                DataAccessMode.ReadOnly => ApplicationDbContextType.ReadOnly,
                DataAccessMode.ReadWrite => ApplicationDbContextType.ReadWrite,
                _ => throw new BugException()
            };
            var isolationLevel = applicationOptions.TransactionMode switch
            {
                TransactionMode.None => IsolationLevel.Unspecified,
                TransactionMode.ReadCommitted => IsolationLevel.ReadCommitted,
                TransactionMode.RepeatableRead => IsolationLevel.RepeatableRead,
                TransactionMode.Serializable => IsolationLevel.Serializable,
                _ => throw new BugException()
            };
            var useTransaction = applicationOptions.TransactionMode != TransactionMode.None;

            return new Options
            {
                DbContextType = dbContextType,
                UseTransaction = useTransaction,
                IsolationLevel = isolationLevel
            };
        }

        public required ApplicationDbContextType DbContextType { get; init; }

        public required bool UseTransaction { get; init; }

        public required IsolationLevel IsolationLevel { get; init; }
    }

    public UnitOfWork(IDbContextFactory<ReadOnlyApplicationDbContext> readOnlyFactory,
        IDbContextFactory<ReadWriteApplicationDbContext> readWriteFactory, IMediator mediator)
    {
        _readOnlyFactory = readOnlyFactory;
        _readWriteFactory = readWriteFactory;
        _mediator = mediator;
        _state = State.NotStarted;
        _dbContext = null;
        _isDisposed = false;
    }

    public async Task Begin(UnitOfWorkOptions applicationOptions, CancellationToken cancellationToken)
    {
        EnsureNotDisposed();

        if (_state != State.NotStarted)
            throw new BugException();
        
        if ((applicationOptions.DataAccessMode == DataAccessMode.ReadOnly) ^ (applicationOptions.TransactionMode == TransactionMode.None))
            throw new BugException();

        BaseApplicationDbContext? dbContext = null;
        try
        {
            var infrastructureOptions = Options.Map(applicationOptions);
            
            dbContext = infrastructureOptions.DbContextType switch
            {
                ApplicationDbContextType.ReadOnly =>
                    await _readOnlyFactory.CreateDbContextAsync(cancellationToken),
                ApplicationDbContextType.ReadWrite =>
                    await _readWriteFactory.CreateDbContextAsync(cancellationToken),
                _ => throw new BugException()
            };

            if (infrastructureOptions.UseTransaction)
                await dbContext.Database.BeginTransactionAsync(infrastructureOptions.IsolationLevel, cancellationToken);
            
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

        await DispatchDomainEvents();

        if (_dbContext!.Database.CurrentTransaction is not null)
            await _dbContext.Database.CommitTransactionAsync(cancellationToken);

        _state = State.NotStarted;

        return;

        async Task DispatchDomainEvents()
        {
            // Максимальная вложенность цепочек событий - 3
            for (var i = 0; i < 3; i++)
            {
                var aggregates = _dbContext!.ChangeTracker.Entries<BaseAggregate>()
                    .Where(e => e.Entity.DomainEvents.Count != 0).Select(e => e.Entity).ToList();

                if (aggregates.Count == 0)
                    return;

                var events = aggregates.SelectMany(a => a.DomainEvents).ToList();

                foreach (var aggregate in aggregates)
                    aggregate.ClearDomainEvents();

                foreach (var domainEvent in events)
                    await _mediator.Publish(domainEvent, cancellationToken);
            }

            throw new BugException();
        }
    }

    public async Task Rollback(CancellationToken cancellationToken)
    {
        EnsureNotDisposed();

        if (_state != State.Begun)
            throw new BugException();

        if (_dbContext!.Database.CurrentTransaction is not null)
            await _dbContext.Database.RollbackTransactionAsync(cancellationToken);

        _state = State.NotStarted;
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

    private readonly IMediator _mediator;

    private State _state;

    private BaseApplicationDbContext? _dbContext;

    private bool _isDisposed;
}