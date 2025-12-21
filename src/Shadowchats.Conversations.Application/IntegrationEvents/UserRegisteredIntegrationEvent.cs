using MediatR;
using Shadowchats.Conversations.Application.Attributes;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

[TracingDecorator]
[LoggingDecorator]
[UnitOfWorkDecorator(DataAccessMode.ReadWrite, TransactionMode.ReadCommitted)]
public sealed record UserRegisteredIntegrationEvent : IIntegrationEvent
{
    private UserRegisteredIntegrationEvent()
    {
        UserId = Guid.Empty;
    }

    public UserRegisteredIntegrationEvent(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; private set; }
    
    string IIntegrationEvent.EventType => EventType;
    
    public const string EventType = "UserRegistered";
}

public sealed class UserRegisteredIntegrationEventHandler : IRequestHandler<UserRegisteredIntegrationEvent, Unit>
{
    public UserRegisteredIntegrationEventHandler(IBuyerRepository buyerRepository, IPersistenceContext persistenceContext)
    {
        _buyerRepository = buyerRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<Unit> Handle(UserRegisteredIntegrationEvent evt, CancellationToken cancellationToken)
    {
        var buyer = new Buyer(evt.UserId);
        await _buyerRepository.Add(buyer, cancellationToken);
        await _persistenceContext.SaveChanges(cancellationToken);
        
        return Unit.Value;
    }
    
    private readonly IBuyerRepository _buyerRepository;
    
    private readonly IPersistenceContext _persistenceContext;
}