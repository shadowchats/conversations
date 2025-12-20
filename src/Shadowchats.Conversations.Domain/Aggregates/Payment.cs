using Shadowchats.Conversations.Domain.DomainEvents;
using Shadowchats.Conversations.Domain.Enums;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.Interfaces;
using Shadowchats.Conversations.Domain.Validators;
using Shadowchats.Conversations.Domain.ValueObjects;

namespace Shadowchats.Conversations.Domain.Aggregates;

public sealed class Payment : BaseAggregate
{
    private Payment()
    {
        OrderId = Guid.Empty;
        Amount = null!;
        Method = 0;
        Status = 0;
    }
    
    private Payment(Guid id, Guid orderId, Money amount, PaymentMethod method, PaymentStatus status) : base(id)
    {
        OrderId = orderId;
        Amount = amount;
        Method = method;
        Status = status;
    }

    public static Payment Create(IGuidGenerator guidGenerator, Guid orderId, Money amount, PaymentMethod method)
    {
        if (amount.Amount <= 0)
            throw new InvariantViolationException("Payment amount must be positive.");
        EnumsValidator.EnsureValid(method);
        
        return new Payment(guidGenerator.Generate(), orderId, amount, method, PaymentStatus.Created);
    }

    public void Complete()
    {
        if (Status != PaymentStatus.Created)
            throw new InvariantViolationException($"Cannot complete payment when status is {Status}.");

        Status = PaymentStatus.Completed;
        AddDomainEvent(new PaymentCompletedDomainEvent(Id, OrderId));
    }
    
    public Guid OrderId { get; private set; }
    
    public Money Amount { get; private set; }
    
    public PaymentMethod Method { get; private set; }
    
    public PaymentStatus Status { get; private set; }
}