using MediatR;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.DomainEvents;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Application.DomainEventHandlers;

public sealed class PaymentCompletedHandler : INotificationHandler<PaymentCompletedDomainEvent>
{
    public PaymentCompletedHandler(IOrderRepository orderRepository, IPersistenceContext persistenceContext, IIntegrationEventPublisher integrationEventPublisher)
    {
        _orderRepository = orderRepository;
        _persistenceContext = persistenceContext;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(PaymentCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.Find(o => o.Id == notification.OrderId, cancellationToken);
        if (order == null)
            throw new InvariantViolationException("Order does not exist.");
        
        order.MarkAsPaid();

        await _persistenceContext.SaveChanges(cancellationToken);

        // Публикация интеграционного события для внешних систем
        await _integrationEventPublisher.Publish(
            new IntegrationEvents.OrderPaidIntegrationEvent(
                notification.OrderId,
                notification.PaymentId,
                DateTime.UtcNow
            ),
            cancellationToken);
    }
    
    private readonly IOrderRepository _orderRepository;
    
    private readonly IPersistenceContext _persistenceContext;
    
    private readonly IIntegrationEventPublisher _integrationEventPublisher;
}