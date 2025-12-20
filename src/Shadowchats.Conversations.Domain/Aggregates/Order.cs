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
        CreatedAt = DateTime.MinValue;
        BuyerId = Guid.Empty;
        Items = null!;
        Status = 0;
        _totalPrice = new Lazy<Money>(() =>
            Items.Any() ? Items.Select(i => i.Total).Aggregate((a, b) => a + b) : Money.None);
    }

    private Order(Guid id, DateTime createdAt, Guid buyerId, ReadOnlyCollection<OrderItem> items, OrderStatus status) : base(id)
    {
        CreatedAt = createdAt;
        BuyerId = buyerId;
        Items = items;
        Status = status;
        _totalPrice = new Lazy<Money>(() => Items.Select(i => i.Total).Aggregate((a, b) => a + b));
    }

    public static Order Create(IGuidGenerator guidGenerator, IDateTimeProvider dateTimeProvider, Guid buyerId, IEnumerable<OrderItem> items)
    {
        var itemsList = items.ToList();
        if (itemsList.Count == 0)
            throw new InvariantViolationException("Order must contain at least one item.");

        var order = new Order(guidGenerator.Generate(), dateTimeProvider.UtcNow, buyerId, itemsList.AsReadOnly(), OrderStatus.Created);

        return order;
    }
    
    public void Cancel()
    {
        if (Status != OrderStatus.Created)
            throw new InvariantViolationException($"Cannot cancel order when status is {Status}.");

        Status = OrderStatus.Cancelled;
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
    
    public DateTime CreatedAt { get; private set; }
    
    public Guid BuyerId { get; private set; }

    public IReadOnlyList<OrderItem> Items { get; private set; }

    public OrderStatus Status { get; private set; }

    public Money TotalPrice => _totalPrice.Value;

    private readonly Lazy<Money> _totalPrice;
}