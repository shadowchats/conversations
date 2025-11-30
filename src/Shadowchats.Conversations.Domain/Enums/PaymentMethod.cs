namespace Shadowchats.Conversations.Domain.Enums;

public enum PaymentMethod : byte
{
    None = 0,
    CryptoTransfer = 1,
    FiatTransfer = 2
}