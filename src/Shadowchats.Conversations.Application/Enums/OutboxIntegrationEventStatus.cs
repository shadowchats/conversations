namespace Shadowchats.Conversations.Application.Enums;

public enum OutboxIntegrationEventStatus : byte
{
    None = 0,
    Pending = 1,
    Published = 2
}