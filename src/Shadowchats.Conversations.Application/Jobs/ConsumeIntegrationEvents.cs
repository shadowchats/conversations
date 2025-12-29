using MediatR;
using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Application.Jobs;

public sealed record ConsumeIntegrationEventsJob : IRequest<int>;

public class ConsumeIntegrationEventsHandler : IRequestHandler<ConsumeIntegrationEventsJob, int>
{
    public ConsumeIntegrationEventsHandler(IInboxIntegrationEventContainerRepository inboxIntegrationEventContainerRepository, IPersistenceContext persistenceContext)
    {
        _inboxIntegrationEventContainerRepository = inboxIntegrationEventContainerRepository;
        _persistenceContext = persistenceContext;
    }

    public Task<int> Handle(ConsumeIntegrationEventsJob _, CancellationToken cancellationToken)
    {
        /*
         * var events = await _integrationEventBroker.Consume(cancellationToken);
         * if (events.Count == 0)
         *     return 0;
         *
         * await _inboxIntegrationEventContainerRepository.Add(events, cancellationToken);
         * await _persistenceContext.SaveChanges(cancellationToken);
         *
         * _integrationEventBroker.Commit(cancellationToken);
         *
         * return events.Count;
         */
        
        throw new NotImplementedException();
    }
    
    private readonly IInboxIntegrationEventContainerRepository _inboxIntegrationEventContainerRepository;
    
    private readonly IPersistenceContext _persistenceContext;
    
    //private readonly IIntegrationEventBroker _integrationEventBroker;
}