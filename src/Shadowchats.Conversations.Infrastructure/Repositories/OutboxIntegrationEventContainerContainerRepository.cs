using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

    public Task<List<OutboxIntegrationEventContainer>> FindAll(
        Expression<Func<OutboxIntegrationEventContainer, bool>> predicate, CancellationToken cancellationToken,
        params Expression<Func<OutboxIntegrationEventContainer, object>>[] includes) => includes
        .Aggregate<Expression<Func<OutboxIntegrationEventContainer, object>>,
            IQueryable<OutboxIntegrationEventContainer>>(_unitOfWork.DbContext.OutboxIntegrationEventContainers,
            (current, include) => current.Include(include)).Where(predicate).ToListAsync(cancellationToken);

    private readonly UnitOfWork _unitOfWork;
}