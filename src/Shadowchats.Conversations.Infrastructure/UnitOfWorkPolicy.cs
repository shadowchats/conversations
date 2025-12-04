using System.Data;
using Shadowchats.Conversations.Application.UseCases.Test1;
using Shadowchats.Conversations.Application.UseCases.Test2;
using Shadowchats.Conversations.Application.UseCases.Test3;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Infrastructure.Enums;

namespace Shadowchats.Conversations.Infrastructure;

public class UnitOfWorkPolicy
{
    public bool RequiresTransaction(Type requestType) => !RequiresTransactionMap.TryGetValue(requestType, out var value) ? throw new BugException($"The request type {requestType} is not supported.") : value;

    public IsolationLevel GetIsolationLevel(Type requestType) => !IsolationLevelMap.TryGetValue(requestType, out var value) ? throw new BugException($"The request type {requestType} is not supported.") : value;

    public ApplicationDbContextType GetDbContextTypeMap(Type requestType) => !DbContextTypeMap.TryGetValue(requestType, out var value) ? throw new BugException($"The request type {requestType} is not supported.") : value;

    private static readonly Dictionary<Type, bool> RequiresTransactionMap = new()
    {
        { typeof(Test1Command), true },
        { typeof(Test2Query), false },
        { typeof(Test3Command), true }
    };

    private static readonly Dictionary<Type, IsolationLevel> IsolationLevelMap = new()
    {
        { typeof(Test1Command), IsolationLevel.ReadCommitted },
        { typeof(Test3Command), IsolationLevel.ReadCommitted }
    };

    private static readonly Dictionary<Type, ApplicationDbContextType> DbContextTypeMap = new()
    {
        { typeof(Test1Command), ApplicationDbContextType.ReadWrite },
        { typeof(Test2Query), ApplicationDbContextType.ReadOnly },
        { typeof(Test3Command), ApplicationDbContextType.ReadWrite }
    };
}