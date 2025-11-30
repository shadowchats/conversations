using Shadowchats.Conversations.Domain.Enums;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Domain.Extensions;

public static class EnumsExtensions
{
    public static void EnsureValid(this Currency currency)
    {
        if (currency is < Currency.Rub or > Currency.Usd)
            throw new InvariantViolationException("Currency must be Rub, Eur or Usd.");
    }
    
    public static void EnsureValid(this PaymentMethod paymentMethod)
    {
        if (paymentMethod is < PaymentMethod.CryptoTransfer or > PaymentMethod.FiatTransfer)
            throw new InvariantViolationException("Payment method must be CryptoTransfer or FiatTransfer.");
    }
}