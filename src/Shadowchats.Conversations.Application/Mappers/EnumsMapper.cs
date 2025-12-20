using Shadowchats.Conversations.Domain.Enums;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Application.Mappers;

internal static class EnumsMapper
{
    public static Currency MapCurrency(string value) => !StringToCurrency.TryGetValue(value, out var currency)
        ? throw new InvariantViolationException("Unknown currency.")
        : currency;
    
    public static string MapCurrency(Currency value) => !CurrencyToString.TryGetValue(value, out var currency)
        ? throw new BugException() : currency;

    public static string MapOrderStatus(OrderStatus value) => !OrderStatusToString.TryGetValue(value, out var status)
        ? throw new BugException()
        : status;
    
    public static PaymentMethod MapPaymentMethod(string value) => !StringToPaymentMethod.TryGetValue(value, out var paymentMethod)
        ? throw new InvariantViolationException("Unknown payment method.")
        : paymentMethod;
    
    public static string MapPaymentMethod(PaymentMethod value) => !PaymentMethodToString.TryGetValue(value, out var paymentMethod)
        ? throw new BugException() : paymentMethod;

    private static readonly Dictionary<string, Currency> StringToCurrency = new()
    {
        { "RUB", Currency.Rub },
        { "EUR", Currency.Eur },
        { "USD", Currency.Usd }
    };
    
    private static readonly Dictionary<Currency, string> CurrencyToString =
        StringToCurrency.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    private static readonly Dictionary<OrderStatus, string> OrderStatusToString = new()
    {
        { OrderStatus.Created, "Created" },
        { OrderStatus.Paid, "Paid" },
        { OrderStatus.Shipped, "Shipped" }
    };
    
    private static readonly Dictionary<string, PaymentMethod> StringToPaymentMethod = new()
    {
        { "CryptoTransfer", PaymentMethod.CryptoTransfer },
        { "FiatTransfer", PaymentMethod.FiatTransfer }
    };
    
    private static readonly Dictionary<PaymentMethod, string> PaymentMethodToString =
        StringToPaymentMethod.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
}