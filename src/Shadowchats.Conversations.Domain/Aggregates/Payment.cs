using Shadowchats.Conversations.Domain.DomainEvents;
using Shadowchats.Conversations.Domain.Enums;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.Extensions;
using Shadowchats.Conversations.Domain.ValueObjects;

namespace Shadowchats.Conversations.Domain.Aggregates;

// Реализация в условиях Entity Framework
public sealed class Payment1 : BaseAggregate1
{
    private Payment1()
    {
        OrderId = Guid.Empty;
        Amount = Money1.None;
        Method = PaymentMethod.None;
    }
    
    public Payment1(Guid id, Guid orderId, Money1 amount, PaymentMethod method) : base(id)
    {
        OrderId = orderId;
        Amount = amount;
        Method = method;
    }

    public static Payment1 Create(Guid id, Guid orderId, Money1 amount, PaymentMethod method)
    {
        if (amount.Amount <= 0)
            throw new InvariantViolationException("Payment amount must be positive.");
        method.EnsureValid();
        
        return new Payment1(id, orderId, amount, method);
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new InvariantViolationException("Payment is already completed.");

        IsCompleted = true;
        AddDomainEvent(new PaymentCompleted(Id, OrderId));
    }
    
    public Guid OrderId { get; private init; }
    
    public Money1 Amount { get; private init; }
    
    public PaymentMethod Method { get; private init; }
    
    public bool IsCompleted { get; private set; }
}

// Реализация в условиях вакуума
public sealed class Payment : BaseAggregate
{
    private Payment(Guid id, Guid orderId, Money amount, PaymentMethod method) : base(id)
    {
        OrderId = orderId;
        Amount = amount;
        Method = method;
    }

    public static Payment Create(Guid id, Guid orderId, Money amount, PaymentMethod method)
    {
        if (amount.Amount <= 0)
            throw new InvariantViolationException("Payment amount must be positive.");
        method.EnsureValid();
        
        return new Payment(id, orderId, amount, method);
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new InvariantViolationException("Payment is already completed.");

        IsCompleted = true;
        AddDomainEvent(new PaymentCompleted(Id, OrderId));
    }
    
    public Guid OrderId { get; }
    
    public Money Amount { get; }
    
    public PaymentMethod Method { get; }
    
    public bool IsCompleted { get; private set; }
}