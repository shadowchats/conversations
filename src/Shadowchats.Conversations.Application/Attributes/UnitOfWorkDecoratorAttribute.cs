using Shadowchats.Conversations.Application.Common;
using Shadowchats.Conversations.Application.Enums;

namespace Shadowchats.Conversations.Application.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class UnitOfWorkDecoratorAttribute : Attribute
{
    public UnitOfWorkDecoratorAttribute(DataAccessMode dataAccess, TransactionMode transaction)
    {
        Options = new UnitOfWorkOptions
        {
            DataAccessMode = dataAccess,
            TransactionMode = transaction
        };
    }

    public UnitOfWorkOptions Options { get; }
}