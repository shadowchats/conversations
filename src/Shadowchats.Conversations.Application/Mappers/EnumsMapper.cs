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

    private static readonly Dictionary<string, Currency> StringToCurrency = new()
    {
        { "RUB", Currency.Rub },
        { "EUR", Currency.Eur },
        { "USD", Currency.Usd }
    };
    
    private static readonly Dictionary<Currency, string> CurrencyToString =
        StringToCurrency.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
}