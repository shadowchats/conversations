using System.Collections.Concurrent;
using System.Reflection;

namespace Shadowchats.Conversations.Application.Attributes;

public static class AttributeCache
{
    public static TAttribute? Get<TAttribute>(Type type) where TAttribute : Attribute
    {
        var key = (type, typeof(TAttribute));
        
        return (TAttribute?)Cache.GetOrAdd(key, _ => type.GetCustomAttribute<TAttribute>());
    }

    private static readonly ConcurrentDictionary<(Type RequestType, Type AttributeType), Attribute?> Cache = new();
}