using System.Collections.ObjectModel;
using Shadowchats.Conversations.Domain.DomainEvents;
using Shadowchats.Conversations.Domain.Entities;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.ValueObjects;

namespace Shadowchats.Conversations.Domain.Aggregates;

// Реализация в условиях Entity Framework
public sealed class Order1 : BaseAggregate1
{
    private Order1()
    {
        Items = new ReadOnlyCollection<OrderItem1>(new List<OrderItem1>());
        IsPaid = false;
        IsShipped = false;
        _totalPrice = new Lazy<Money1>(() =>
            Items.Any() ? Items.Select(i => i.Total).Aggregate((a, b) => a + b) : Money1.None);
    }

    private Order1(Guid id, ReadOnlyCollection<OrderItem1> items, bool isPaid, bool isShipped) : base(id)
    {
        Items = items;
        IsPaid = isPaid;
        IsShipped = isShipped;
        _totalPrice = new Lazy<Money1>(() => Items.Select(i => i.Total).Aggregate((a, b) => a + b));
    }

    public static Order1 Create(Guid id, IEnumerable<OrderItem1> items)
    {
        var itemsList = items.ToList();
        if (itemsList.Count == 0)
            throw new InvariantViolationException("Order must contain at least one item.");

        var order = new Order1(id, itemsList.AsReadOnly(), false, false);
        order.AddDomainEvent(new OrderPlacedDomainEvent(order.Id));

        return order;
    }

    public void MarkAsPaid()
    {
        if (IsPaid)
            throw new InvariantViolationException("Order is already paid.");

        IsPaid = true;
        AddDomainEvent(new OrderPaidDomainEvent(Id));
    }

    public void Ship()
    {
        if (!IsPaid)
            throw new InvariantViolationException("Cannot ship an unpaid order.");
        if (IsShipped)
            throw new InvariantViolationException("Order is already shipped.");

        IsShipped = true;
    }

    public IReadOnlyList<OrderItem1> Items { get; private init; }

    public bool IsPaid { get; private set; }

    public bool IsShipped { get; private set; }

    public Money1 TotalPrice => _totalPrice.Value;

    private readonly Lazy<Money1> _totalPrice;
}

// Реализация в условиях вакуума
public sealed class Order : BaseAggregate
{
    private Order(Guid id, ReadOnlyCollection<OrderItem> items, bool isPaid, bool isShipped) : base(id)
    {
        Items = items;
        IsPaid = isPaid;
        IsShipped = isShipped;
        _totalPrice = new Lazy<Money>(() => Items.Select(i => i.Total).Aggregate((a, b) => a + b));
    }

    public static Order Create(Guid id, IEnumerable<OrderItem> items)
    {
        var itemsList = items.ToList();
        if (itemsList.Count == 0)
            throw new InvariantViolationException("Order must contain at least one item.");

        var order = new Order(id, itemsList.AsReadOnly(), false, false);
        order.AddDomainEvent(new OrderPlacedDomainEvent(order.Id));

        return order;
    }

    public void MarkAsPaid()
    {
        if (IsPaid)
            throw new InvariantViolationException("Order is already paid.");

        IsPaid = true;
        AddDomainEvent(new OrderPaidDomainEvent(Id));
    }

    public void Ship()
    {
        if (!IsPaid)
            throw new InvariantViolationException("Cannot ship an unpaid order.");
        if (IsShipped)
            throw new InvariantViolationException("Order is already shipped.");

        IsShipped = true;
    }

    public IReadOnlyList<OrderItem> Items { get; }

    public bool IsPaid { get; private set; }

    public bool IsShipped { get; private set; }

    public Money TotalPrice => _totalPrice.Value;

    private readonly Lazy<Money> _totalPrice;
}