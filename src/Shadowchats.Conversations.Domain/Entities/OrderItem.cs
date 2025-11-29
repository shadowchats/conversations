using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.ValueObjects;

namespace Shadowchats.Conversations.Domain.Entities;

// Реализация в условиях Entity Framework
public sealed class OrderItem1 : BaseEntity1
{
    private OrderItem1()
    {
        ProductId = Guid.Empty;
        Quantity = 0;
        Price = null!;
        _total = new Lazy<Money>(() => Money.Create(Price.Amount * Quantity, Price.Currency));
    }
    
    private OrderItem1(Guid id, Guid productId, int quantity, Money price) : base(id)
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        _total = new Lazy<Money>(() => Money.Create(Price.Amount * Quantity, Price.Currency));
    }

    public static OrderItem1 Create(Guid id, Guid productId, int quantity, Money price) => quantity <= 0
        ? throw new InvariantViolationException("Quantity must be > 0.")
        : new OrderItem1(id, productId, quantity, price);

    public Guid ProductId { get; private init; }
    
    public int Quantity { get; private init; }
    
    public Money Price { get; private init; }

    public Money Total => _total.Value;
    
    private readonly Lazy<Money> _total;
}

// Реализация в условиях вакуума
public sealed class OrderItem : BaseEntity
{
    private OrderItem(Guid id, Guid productId, int quantity, Money price) : base(id)
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        _total = new Lazy<Money>(() => Money.Create(Price.Amount * Quantity, Price.Currency));
    }

    public static OrderItem Create(Guid id, Guid productId, int quantity, Money price) => quantity <= 0
        ? throw new InvariantViolationException("Quantity must be > 0.")
        : new OrderItem(id, productId, quantity, price);

    public Guid ProductId { get; }
    
    public int Quantity { get; }
    
    public Money Price { get; }

    public Money Total => _total.Value;
    
    private readonly Lazy<Money> _total;
}