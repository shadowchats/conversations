using Microsoft.EntityFrameworkCore;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.IntegrationEvents;
using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public class OutboxIntegrationEventContainerContainerRepository : IOutboxIntegrationEventContainerRepository
{
    public OutboxIntegrationEventContainerContainerRepository(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task Add(OutboxIntegrationEventContainer integrationEventContainer, CancellationToken cancellationToken) =>
        _unitOfWork.DbContext.OutboxIntegrationEventContainers.AddAsync(integrationEventContainer, cancellationToken)
            .AsTask();

    public Task<List<OutboxIntegrationEventContainer>> TakePendingBatch(int batchSize,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT *
                           FROM "OutboxIntegrationEventContainers"
                           WHERE "Status" = {0}
                           ORDER BY "CreatedAt"
                           FOR UPDATE SKIP LOCKED
                           LIMIT {1};
                           """;

        return _unitOfWork.DbContext.OutboxIntegrationEventContainers
            .FromSqlRaw(sql, OutboxIntegrationEventStatus.Pending, batchSize).ToListAsync(cancellationToken);
    }

    private readonly UnitOfWork _unitOfWork;
}