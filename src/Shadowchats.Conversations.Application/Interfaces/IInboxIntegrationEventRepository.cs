using Shadowchats.Conversations.Application.IntegrationEvents;

namespace Shadowchats.Conversations.Application.Interfaces;

public interface IInboxIntegrationEventRepository
{
    Task Add(IEnumerable<InboxIntegrationEventContainer> integrationEventContainer, CancellationToken cancellationToken);
}