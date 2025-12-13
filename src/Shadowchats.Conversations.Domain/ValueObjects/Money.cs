using Shadowchats.Conversations.Domain.Enums;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.Validators;

namespace Shadowchats.Conversations.Domain.ValueObjects;

// Реализация в условиях Entity Framework
public sealed record Money1
{
    private Money1()
    {
        Amount = 0;
        Currency = Currency.None;
    }
    
    private Money1(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money1 Create(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new InvariantViolationException("Amount must be >= 0.");
        EnumsValidator.EnsureValid(currency);
        
        return new Money1(amount, currency);
    }

    public static Money1 operator +(Money1 left, Money1 right) => left.Currency != right.Currency
        ? throw new InvariantViolationException("Currencies must match.")
        : new Money1(left.Amount + right.Amount, left.Currency);
    
    public decimal Amount { get; private init; }
    
    public Currency Currency { get; private init; }
    
    public static readonly Money1 None = new();
}

// Реализация в условиях вакуума
public sealed record Money
{
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

    public decimal Amount { get; }

    public Currency Currency { get; }
}