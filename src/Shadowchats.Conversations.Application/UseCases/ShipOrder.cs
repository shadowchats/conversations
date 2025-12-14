using MediatR;
using Shadowchats.Conversations.Application.Attributes;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Application.UseCases;

[UnitOfWork(DataAccessMode.ReadWrite, TransactionMode.ReadCommitted)]
public sealed record ShipOrderCommand : IRequest<Unit>
{
    public required Guid OrderId { get; init; }
}

public sealed class ShipOrderHandler : IRequestHandler<ShipOrderCommand, Unit>
{
    public ShipOrderHandler(IOrderRepository orderRepository, IPersistenceContext persistenceContext)
    {
        _orderRepository = orderRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<Unit> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.Find(o => o.Id == request.OrderId, cancellationToken);
        if (order == null)
            throw new InvariantViolationException("Order does not exist.");

        order.Ship();
        
        await _persistenceContext.SaveChanges(cancellationToken);

        return Unit.Value;
    }
    
    private readonly IOrderRepository _orderRepository;
    
    private readonly IPersistenceContext _persistenceContext;
}