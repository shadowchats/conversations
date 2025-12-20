using MediatR;
using Shadowchats.Conversations.Application.Attributes;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Application.Mappers;
using Shadowchats.Conversations.Domain.Aggregates;
using Shadowchats.Conversations.Domain.Entities;
using Shadowchats.Conversations.Domain.Interfaces;
using Shadowchats.Conversations.Domain.ValueObjects;

namespace Shadowchats.Conversations.Application.UseCases;

[UnitOfWork(DataAccessMode.ReadWrite, TransactionMode.ReadCommitted)]
public sealed record CreateOrderCommand : IRequest<CreateOrderResponse>
{
    public required Guid BuyerId { get; init; }

    public required List<CreateOrderItemDto> Items { get; init; }
}

public sealed record CreateOrderItemDto
{
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
    public CreateOrderHandler(IGuidGenerator guidGenerator, IOrderRepository orderRepository, IPersistenceContext persistenceContext)
    {
        _guidGenerator = guidGenerator;
        _orderRepository = orderRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var items = request.Items.Select(dto => OrderItem.Create(
            _guidGenerator,
            dto.ProductId,
            dto.Quantity,
            Money.Create(dto.Amount, EnumsMapper.MapCurrency(dto.Currency))
        )).ToList();

        var order = Order.Create(_guidGenerator, request.BuyerId, items);
        await _orderRepository.Add(order, cancellationToken);
        await _persistenceContext.SaveChanges(cancellationToken);
        
        return new CreateOrderResponse { OrderId = order.Id };
    }
    
    private readonly IGuidGenerator _guidGenerator;
    
    private readonly IOrderRepository _orderRepository;
    
    private readonly IPersistenceContext _persistenceContext;
}