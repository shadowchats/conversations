using MediatR;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Application.Jobs;

public sealed record PublishIntegrationEventsJob : IRequest<Unit>;

public sealed class PublishIntegrationEventsHandler : IRequestHandler<PublishIntegrationEventsJob, Unit>
{
    public PublishIntegrationEventsHandler(IOutboxIntegrationEventContainerRepository outboxIntegrationEventContainerRepository, IPersistenceContext persistenceContext)
    {
        _outboxIntegrationEventContainerRepository = outboxIntegrationEventContainerRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<Unit> Handle(PublishIntegrationEventsJob request, CancellationToken cancellationToken)
    {
        // TODO: *ОБЯЗАТЕЛЬНО* реализовать `FOR UPDATE SKIP LOCKED` и лимитирование подтягиваемых событий (допустим, по сотне)
        var pendingEvents =
            await _outboxIntegrationEventContainerRepository.FindAll(
                c => c.Status == OutboxIntegrationEventStatus.Pending, cancellationToken);
        
        //_integrationEventBroker.Publish(pendingEvents);
        
        pendingEvents.ForEach(e => e.MarkAsPublished());

        await _persistenceContext.SaveChanges(cancellationToken);
        
        return Unit.Value;
    }
    
    private readonly IOutboxIntegrationEventContainerRepository _outboxIntegrationEventContainerRepository;
    
    private readonly IPersistenceContext _persistenceContext;
    
    //private readonly IIntegrationEventBroker _integrationEventBroker;
}