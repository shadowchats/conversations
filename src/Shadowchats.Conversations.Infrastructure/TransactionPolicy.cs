using System.Data;
using Shadowchats.Conversations.Infrastructure.Interfaces;

namespace Shadowchats.Conversations.Infrastructure;

public class TransactionPolicy : ITransactionPolicy
{
    public IsolationLevel GetIsolationLevel(Type commandType)
    {
        throw new NotImplementedException();
    }
}