using Shadowchats.Conversations.Application.IntegrationEvents;
using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public class OutboxIntegrationEventContainerRepository : BaseRepository<OutboxIntegrationEventContainer>, IOutboxIntegrationEventContainerRepository
{
    public OutboxIntegrationEventContainerRepository(UnitOfWork unitOfWork) : base(unitOfWork) { }
}