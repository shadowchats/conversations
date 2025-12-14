using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Application.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class UnitOfWorkAttribute : Attribute
{
    public UnitOfWorkAttribute(DataAccessMode dataAccess, TransactionMode transaction)
    {
        if ((dataAccess == DataAccessMode.ReadOnly) ^ (transaction == TransactionMode.None))
            throw new BugException();

        DataAccess = dataAccess;
        Transaction = transaction;
    }

    public DataAccessMode DataAccess { get; }

    public TransactionMode Transaction { get; }
}