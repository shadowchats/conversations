using MediatR;
using Shadowchats.Conversations.Application.Attributes;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Application.Mappers;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Application.UseCases;

[TracingDecorator]
[LoggingDecorator]
[UnitOfWorkDecorator(DataAccessMode.ReadOnly, TransactionMode.None)]
public sealed record GetOrderQuery : IRequest<GetOrderResponse>
{
    public required Guid OrderId { get; init; }
}

public sealed record GetOrderResponse
{
    public required Guid OrderId { get; init; }

    public required List<OrderItemDto> Items { get; init; }

    public required string Status { get; init; }

    public required decimal TotalAmount { get; init; }

    public required string Currency { get; init; }
}

public sealed record OrderItemDto
{
    public required Guid ItemId { get; init; }

    public required Guid ProductId { get; init; }

    public required int Quantity { get; init; }

    public required decimal Amount { get; init; }

    public required string Currency { get; init; }
}

public sealed class GetOrderHandler : IRequestHandler<GetOrderQuery, GetOrderResponse>
{
    public GetOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<GetOrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.Find(o => o.Id == request.OrderId, cancellationToken, includes: o => o.Items);
        if (order == null)
            throw new InvariantViolationException("Order does not exist.");

        var items = order.Items.Select(i => new OrderItemDto
        {
            ItemId = i.Id, ProductId = i.ProductId, Quantity = i.Quantity, Amount = i.Price.Amount,
            Currency = EnumsMapper.MapCurrency(i.Price.Currency)
        }).ToList();

        return new GetOrderResponse
        {
            OrderId = order.Id, Items = items, Status = EnumsMapper.MapOrderStatus(order.Status),
            TotalAmount = order.TotalPrice.Amount, Currency = EnumsMapper.MapCurrency(order.TotalPrice.Currency)
        };
    }

    private readonly IOrderRepository _orderRepository;
}