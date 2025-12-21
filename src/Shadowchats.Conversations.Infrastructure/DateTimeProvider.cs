using Shadowchats.Conversations.Domain.Interfaces;

namespace Shadowchats.Conversations.Infrastructure;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}