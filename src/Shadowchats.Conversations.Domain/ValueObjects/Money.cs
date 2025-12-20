using Shadowchats.Conversations.Domain.Enums;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.Validators;

namespace Shadowchats.Conversations.Domain.ValueObjects;

public sealed record Money
{
    private Money()
    {
        Amount = 0;
        Currency = 0;
    }
    
    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new InvariantViolationException("Amount must be >= 0.");
        EnumsValidator.EnsureValid(currency);
        
        return new Money(amount, currency);
    }

    public static Money operator +(Money left, Money right) => left.Currency != right.Currency
        ? throw new InvariantViolationException("Currencies must match.")
        : new Money(left.Amount + right.Amount, left.Currency);
    
    public decimal Amount { get; private set; }
    
    public Currency Currency { get; private set; }

    public static readonly Money None = Create(0, Currency.Usd);
}