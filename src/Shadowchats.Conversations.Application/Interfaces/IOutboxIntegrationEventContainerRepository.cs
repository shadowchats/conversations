using System.Linq.Expressions;
using Shadowchats.Conversations.Application.IntegrationEvents;

namespace Shadowchats.Conversations.Application.Interfaces;

public interface IOutboxIntegrationEventContainerRepository
{
    Task Add(OutboxIntegrationEventContainer integrationEventContainer, CancellationToken cancellationToken);

    Task<List<OutboxIntegrationEventContainer>> FindAll(
        Expression<Func<OutboxIntegrationEventContainer, bool>> predicate, CancellationToken cancellationToken,
        params Expression<Func<OutboxIntegrationEventContainer, object>>[] includes);
}