using Shadowchats.Conversations.Application.IntegrationEvents;
using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public class InboxIntegrationEventContainerContainerRepository : IInboxIntegrationEventContainerRepository
{
    public InboxIntegrationEventContainerContainerRepository(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task Add(IEnumerable<InboxIntegrationEventContainer> integrationEventContainers,
        CancellationToken cancellationToken) =>
        _unitOfWork.DbContext.InboxIntegrationEventContainers.AddRangeAsync(integrationEventContainers,
            cancellationToken);

    private readonly UnitOfWork _unitOfWork;
}