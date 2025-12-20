using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    public OrderRepository(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task Add(Order order, CancellationToken cancellationToken) =>
        _unitOfWork.DbContext.Orders.AddAsync(order, cancellationToken).AsTask();

    public Task<Order?> Find(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken,
        params Expression<Func<Order, object>>[] includes) => includes
        .Aggregate<Expression<Func<Order, object>>, IQueryable<Order>>(_unitOfWork.DbContext.Orders,
            (current, include) => current.Include(include)).FirstOrDefaultAsync(predicate, cancellationToken);

    public Task<List<Order>> FindAll(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken,
        params Expression<Func<Order, object>>[] includes) => includes
        .Aggregate<Expression<Func<Order, object>>, IQueryable<Order>>(_unitOfWork.DbContext.Orders,
            (current, include) => current.Include(include)).Where(predicate).ToListAsync(cancellationToken);

    private readonly UnitOfWork _unitOfWork;
}