using Shadowchats.Conversations.Application.IntegrationEvents;
using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public class OutboxIntegrationEventContainerRepository : IOutboxIntegrationEventRepository
{
    public OutboxIntegrationEventContainerRepository(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task Add(OutboxIntegrationEventContainer integrationEventContainer, CancellationToken cancellationToken) =>
        _unitOfWork.DbContext.OutboxIntegrationEventContainers.AddAsync(integrationEventContainer, cancellationToken)
            .AsTask();

    private readonly UnitOfWork _unitOfWork;
}