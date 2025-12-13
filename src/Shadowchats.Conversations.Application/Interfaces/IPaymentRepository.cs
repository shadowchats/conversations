using System.Linq.Expressions;
using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Application.Interfaces;

public interface IPaymentRepository
{
    Task Add(Payment payment, CancellationToken cancellationToken);

    Task<Payment?> Find(Expression<Func<Payment, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<Payment, object>>[] includes);
}