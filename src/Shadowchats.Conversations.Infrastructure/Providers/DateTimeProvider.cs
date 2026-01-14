using Shadowchats.Conversations.Domain.Interfaces;

namespace Shadowchats.Conversations.Infrastructure.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}