using System.Linq.Expressions;
using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Application.Interfaces;

public interface IOrderRepository
{
    Task Add(Order order, CancellationToken cancellationToken);

    Task<Order?> Find(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<Order, object>>[] includes);
    
    Task<List<Order>> FindAll(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<Order, object>>[] includes);
}