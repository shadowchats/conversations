namespace Shadowchats.Conversations.Domain.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}