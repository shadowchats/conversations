using MediatR;
using Shadowchats.Conversations.Application.Attributes;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Application.Mappers;
using Shadowchats.Conversations.Domain.Aggregates;
using Shadowchats.Conversations.Domain.Entities;
using Shadowchats.Conversations.Domain.ValueObjects;

namespace Shadowchats.Conversations.Application.UseCases;

[UnitOfWork(DataAccessMode.ReadWrite, TransactionMode.ReadCommitted)]
public sealed record CreateOrderCommand : IRequest<CreateOrderResponse>
{
    public required Guid OrderId { get; init; }

    public required List<CreateOrderItemDto> Items { get; init; }
}

public sealed record CreateOrderItemDto
{
    public required Guid ItemId { get; init; }
    
    public required Guid ProductId { get; init; }
    
    public required int Quantity { get; init; }
    
    public required decimal Amount { get; init; }
    
    public required string Currency { get; init; }
}

public sealed record CreateOrderResponse
{
    public required Guid OrderId { get; init; }
}

public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    public CreateOrderHandler(IOrderRepository orderRepository, IPersistenceContext persistenceContext)
    {
        _orderRepository = orderRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var items = request.Items.Select(dto => OrderItem.Create(
            dto.ItemId,
            dto.ProductId,
            dto.Quantity,
            Money.Create(dto.Amount, EnumsMapper.MapCurrency(dto.Currency))
        )).ToList();

        var order = Order.Create(request.OrderId, items);
        await _orderRepository.Add(order, cancellationToken);
        await _persistenceContext.SaveChanges(cancellationToken);
        
        return new CreateOrderResponse { OrderId = order.Id };
    }
    
    private readonly IOrderRepository _orderRepository;
    
    private readonly IPersistenceContext _persistenceContext;
}