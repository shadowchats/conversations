using Shadowchats.Conversations.Application.Enums;

namespace Shadowchats.Conversations.Application.Common;

public sealed record UnitOfWorkOptions
{
    public required DataAccessMode DataAccessMode { get; init; }

    public required TransactionMode TransactionMode { get; init; }
}