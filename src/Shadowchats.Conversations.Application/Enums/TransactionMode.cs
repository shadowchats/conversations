namespace Shadowchats.Conversations.Application.Enums;

public enum TransactionMode : byte
{
    None = 0,
    ReadCommitted = 1,
    RepeatableRead = 2,
    Serializable = 3
}