using Shadowchats.Conversations.Application.IntegrationEvents;

namespace Shadowchats.Conversations.Application.Interfaces;

public interface IInboxIntegrationEventContainerRepository
{
    Task Add(IEnumerable<InboxIntegrationEventContainer> integrationEventContainer, CancellationToken cancellationToken);
}