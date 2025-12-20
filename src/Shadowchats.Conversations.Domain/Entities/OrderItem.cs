using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.Interfaces;
using Shadowchats.Conversations.Domain.ValueObjects;

namespace Shadowchats.Conversations.Domain.Entities;

public sealed class OrderItem : BaseEntity
{
    private OrderItem()
    {
        ProductId = Guid.Empty;
        Quantity = 0;
        Price = null!;
        _total = new Lazy<Money>(() => Money.Create(Price.Amount * Quantity, Price.Currency));
    }
    
    private OrderItem(Guid id, Guid productId, int quantity, Money price) : base(id)
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        _total = new Lazy<Money>(() => Money.Create(Price.Amount * Quantity, Price.Currency));
    }

    public static OrderItem Create(IGuidGenerator guidGenerator, Guid productId, int quantity, Money price) => quantity <= 0
        ? throw new InvariantViolationException("Quantity must be > 0.")
        : new OrderItem(guidGenerator.Generate(), productId, quantity, price);

    public Guid ProductId { get; private init; }
    
    public int Quantity { get; private init; }
    
    public Money Price { get; private init; }

    public Money Total => _total.Value;
    
    private readonly Lazy<Money> _total;
}