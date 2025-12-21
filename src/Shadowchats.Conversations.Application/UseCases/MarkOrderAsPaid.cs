using MediatR;
using Shadowchats.Conversations.Application.Attributes;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Application.UseCases;

[TracingDecorator]
[LoggingDecorator]
[UnitOfWorkDecorator(DataAccessMode.ReadWrite, TransactionMode.ReadCommitted)]
public sealed record MarkOrderAsPaidCommand : IRequest<Unit>
{
    public required Guid OrderId { get; init; }
}

public sealed class MarkOrderAsPaidHandler : IRequestHandler<MarkOrderAsPaidCommand, Unit>
{
    public MarkOrderAsPaidHandler(IOrderRepository orderRepository, IPersistenceContext persistenceContext)
    {
        _orderRepository = orderRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<Unit> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.Find(o => o.Id == request.OrderId, cancellationToken);
        if (order == null)
            throw new InvariantViolationException("Order does not exist.");
        
        order.MarkAsPaid();
        
        await _persistenceContext.SaveChanges(cancellationToken);

        return Unit.Value;
    }
    
    private readonly IOrderRepository _orderRepository;
    
    private readonly IPersistenceContext _persistenceContext;
}