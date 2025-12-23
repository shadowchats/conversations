using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public sealed class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(UnitOfWork unitOfWork) : base(unitOfWork) { }
}