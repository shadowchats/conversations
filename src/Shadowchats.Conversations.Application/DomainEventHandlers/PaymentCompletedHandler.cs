using MediatR;
using Shadowchats.Conversations.Application.IntegrationEvents;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.DomainEvents;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.Interfaces;

namespace Shadowchats.Conversations.Application.DomainEventHandlers;

public sealed class PaymentCompletedHandler : IRequestHandler<PaymentCompletedDomainEvent, Unit>
{
    public PaymentCompletedHandler(IGuidGenerator guidGenerator, IDateTimeProvider dateTimeProvider,
        ITraceparentProvider traceparentProvider, IOrderRepository orderRepository,
        IOutboxIntegrationEventContainerRepository outboxIntegrationEventContainerRepository,
        IPersistenceContext persistenceContext)
    {
        _guidGenerator = guidGenerator;
        _dateTimeProvider = dateTimeProvider;
        _traceparentProvider = traceparentProvider;
        _orderRepository = orderRepository;
        _outboxIntegrationEventContainerRepository = outboxIntegrationEventContainerRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<Unit> Handle(PaymentCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.Find(o => o.Id == notification.OrderId, cancellationToken);
        if (order == null)
            throw new InvariantViolationException("Order does not exist.");
        order.MarkAsPaid();

        var orderPaidIntegrationEvent = new OrderPaidIntegrationEvent(notification.OrderId, notification.PaymentId);
        var outboxIntegrationEventContainer = OutboxIntegrationEventContainer.Create(_guidGenerator, _dateTimeProvider,
            _traceparentProvider, orderPaidIntegrationEvent);
        await _outboxIntegrationEventContainerRepository.Add(outboxIntegrationEventContainer, cancellationToken);

        await _persistenceContext.SaveChanges(cancellationToken);

        return Unit.Value;
    }

    private readonly IGuidGenerator _guidGenerator;

    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly ITraceparentProvider _traceparentProvider;

    private readonly IOrderRepository _orderRepository;

    private readonly IPersistenceContext _persistenceContext;

    private readonly IOutboxIntegrationEventContainerRepository _outboxIntegrationEventContainerRepository;
}