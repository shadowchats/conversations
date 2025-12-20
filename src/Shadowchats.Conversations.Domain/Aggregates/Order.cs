using System.Collections.ObjectModel;
using Shadowchats.Conversations.Domain.Entities;
using Shadowchats.Conversations.Domain.Enums;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.Interfaces;
using Shadowchats.Conversations.Domain.ValueObjects;

namespace Shadowchats.Conversations.Domain.Aggregates;

public sealed class Order : BaseAggregate
{
    private Order()
    {
        BuyerId = Guid.Empty;
        Items = null!;
        Status = 0;
        _totalPrice = new Lazy<Money>(() =>
            Items.Any() ? Items.Select(i => i.Total).Aggregate((a, b) => a + b) : Money.None);
    }

    private Order(Guid id, Guid buyerId, ReadOnlyCollection<OrderItem> items, OrderStatus status) : base(id)
    {
        BuyerId = buyerId;
        Items = items;
        Status = status;
        _totalPrice = new Lazy<Money>(() => Items.Select(i => i.Total).Aggregate((a, b) => a + b));
    }

    public static Order Create(IGuidGenerator guidGenerator, Guid buyerId, IEnumerable<OrderItem> items)
    {
        var itemsList = items.ToList();
        if (itemsList.Count == 0)
            throw new InvariantViolationException("Order must contain at least one item.");

        var order = new Order(guidGenerator.Generate(), buyerId, itemsList.AsReadOnly(), OrderStatus.Created);

        return order;
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Created)
            throw new InvariantViolationException($"Cannot mark order as paid when status is {Status}.");

        Status = OrderStatus.Paid;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Paid)
            throw new InvariantViolationException($"Cannot ship order when status is {Status}.");

        Status = OrderStatus.Shipped;
    }
    
    public Guid BuyerId { get; private set; }

    public IReadOnlyList<OrderItem> Items { get; private set; }

    public OrderStatus Status { get; private set; }

    public Money TotalPrice => _totalPrice.Value;

    private readonly Lazy<Money> _totalPrice;
}