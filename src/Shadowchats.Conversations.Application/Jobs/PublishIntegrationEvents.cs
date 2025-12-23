using MediatR;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Application.Jobs;

public sealed record PublishIntegrationEventsJob : IRequest<int>;

public sealed class PublishIntegrationEventsHandler : IRequestHandler<PublishIntegrationEventsJob, int>
{
    public PublishIntegrationEventsHandler(IOutboxIntegrationEventContainerRepository outboxIntegrationEventContainerRepository, IPersistenceContext persistenceContext)
    {
        _outboxIntegrationEventContainerRepository = outboxIntegrationEventContainerRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<int> Handle(PublishIntegrationEventsJob _, CancellationToken cancellationToken)
    {
        var pendingEvents = await _outboxIntegrationEventContainerRepository.FindAll(
            e => e.Status == OutboxIntegrationEventStatus.Pending, cancellationToken, 100, LockMode.ForUpdateSkipLocked,
            q => q.OrderBy(e => e.CreatedAt));
        if (pendingEvents.Count == 0)
            return 0;
        
        //_integrationEventBroker.Publish(pendingEvents);
        
        pendingEvents.ForEach(e => e.MarkAsPublished());

        await _persistenceContext.SaveChanges(cancellationToken);
        
        return pendingEvents.Count;
    }
    
    private readonly IOutboxIntegrationEventContainerRepository _outboxIntegrationEventContainerRepository;
    
    private readonly IPersistenceContext _persistenceContext;
    
    //private readonly IIntegrationEventBroker _integrationEventBroker;
}