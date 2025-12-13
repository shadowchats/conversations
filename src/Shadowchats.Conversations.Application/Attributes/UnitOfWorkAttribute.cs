using Shadowchats.Conversations.Application.Enums;

namespace Shadowchats.Conversations.Application.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class UnitOfWorkAttribute : Attribute
{
    public required DataAccessMode DataAccess { get; init; }

    public required TransactionMode Transaction { get; init; }
}