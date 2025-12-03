using System.Data;

namespace Shadowchats.Conversations.Infrastructure.Interfaces;

public interface ITransactionPolicy
{
    IsolationLevel GetIsolationLevel(Type commandType);
}