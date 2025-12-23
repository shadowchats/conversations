using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public sealed class BuyerRepository : BaseRepository<Buyer>, IBuyerRepository
{
    public BuyerRepository(UnitOfWork unitOfWork) : base(unitOfWork) { }
}