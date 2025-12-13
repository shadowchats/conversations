using Shadowchats.Conversations.Domain.Enums;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Domain.Validators;

public static class EnumsValidator
{
    public static void EnsureValid(Currency currency)
    {
        if (currency is < Currency.Rub or > Currency.Usd)
            throw new InvariantViolationException("Unknown currency.");
    }
    
    public static void EnsureValid(PaymentMethod paymentMethod)
    {
        if (paymentMethod is < PaymentMethod.CryptoTransfer or > PaymentMethod.FiatTransfer)
            throw new InvariantViolationException("Unknown payment method.");
    }
}