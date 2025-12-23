using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public sealed class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(UnitOfWork unitOfWork) : base(unitOfWork) { }
}