using Shadowchats.Conversations.Application.IntegrationEvents;

namespace Shadowchats.Conversations.Application.Interfaces;

public interface IOutboxIntegrationEventRepository
{
    Task Add(OutboxIntegrationEventContainer integrationEventContainer, CancellationToken cancellationToken);
}