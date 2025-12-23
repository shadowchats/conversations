using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Infrastructure.Extensions;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected BaseRepository(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task Add(TEntity entity, CancellationToken cancellationToken) =>
        _unitOfWork.DbContext.Set<TEntity>().AddAsync(entity, cancellationToken).AsTask();

    public Task<TEntity?> Find(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken,
        LockMode lockMode = LockMode.None,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = BuildQuery(predicate, queryModifier, includes, lockMode);
        
        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public Task<List<TEntity>> FindAll(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken,
        int batchSize = int.MaxValue,
        LockMode lockMode = LockMode.None,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = BuildQuery(predicate, queryModifier, includes, lockMode);

        if (batchSize != int.MaxValue)
            query = query.Take(batchSize);

        return query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Собирает базовый IQueryable с фильтром, Includes, queryModifier и LockMode.
    /// </summary>
    private IQueryable<TEntity> BuildQuery(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier,
        Expression<Func<TEntity, object>>[] includes,
        LockMode lockMode)
    {
        var query = _unitOfWork.DbContext.Set<TEntity>().Where(predicate);

        if (queryModifier != null)
            query = queryModifier(query);

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return ApplyLockMode(query, lockMode);
    }

    private static IQueryable<TEntity> ApplyLockMode(IQueryable<TEntity> query, LockMode lockMode)
    {
        return lockMode switch
        {
            LockMode.None => query,
            LockMode.ForUpdateSkipLocked => query.ForUpdateSkipLocked(),
            _ => throw new BugException()
        };
    }
    
    private readonly UnitOfWork _unitOfWork;
}