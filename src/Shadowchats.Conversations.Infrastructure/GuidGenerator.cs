using Shadowchats.Conversations.Domain.Interfaces;

namespace Shadowchats.Conversations.Infrastructure;

public class GuidGenerator : IGuidGenerator
{
    public Guid Generate() => Guid.NewGuid();
}