using Shadowchats.Conversations.Application.IntegrationEvents;
using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public class InboxIntegrationEventContainerRepository : BaseRepository<InboxIntegrationEventContainer>, IInboxIntegrationEventContainerRepository
{
    public InboxIntegrationEventContainerRepository(UnitOfWork unitOfWork) : base(unitOfWork) { }
}