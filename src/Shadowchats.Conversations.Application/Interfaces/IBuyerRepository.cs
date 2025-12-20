using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Application.Interfaces;

public interface IBuyerRepository
{
    Task Add(Buyer buyer, CancellationToken cancellationToken);
}