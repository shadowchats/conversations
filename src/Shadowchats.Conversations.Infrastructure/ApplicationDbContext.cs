using Microsoft.EntityFrameworkCore;

namespace Shadowchats.Conversations.Infrastructure;

public abstract class BaseApplicationDbContext : DbContext
{
    
}

public sealed class ReadOnlyApplicationDbContext : BaseApplicationDbContext
{
    
}

public sealed class ReadWriteApplicationDbContext : BaseApplicationDbContext
{
    
}