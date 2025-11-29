using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Domain.Enums;

public static class EnumsValidator
{
    public static void Validate(Currency currency)
    {
        // ReSharper disable once PatternIsRedundant
        if (currency is < Currency.Rub or > Currency.Usd)
            throw new InvariantViolationException("Currency must be Rub, Eur or Usd.");
    }
}