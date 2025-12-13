using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public sealed class PaymentRepository : IPaymentRepository
{
    public PaymentRepository(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task Add(Payment payment, CancellationToken cancellationToken) =>
        _unitOfWork.DbContext.Payments.AddAsync(payment, cancellationToken).AsTask();

    public Task<Payment?> Find(Expression<Func<Payment, bool>> predicate, CancellationToken cancellationToken,
        params Expression<Func<Payment, object>>[] includes) => includes
        .Aggregate<Expression<Func<Payment, object>>, IQueryable<Payment>>(_unitOfWork.DbContext.Payments,
            (current, include) => current.Include(include)).FirstOrDefaultAsync(predicate, cancellationToken);

    private readonly UnitOfWork _unitOfWork;
}