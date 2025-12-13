using Microsoft.EntityFrameworkCore;
using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Infrastructure;

public abstract class BaseApplicationDbContext : DbContext
{
    public DbSet<Order> Orders { get; private set; } = null!;
    
    public DbSet<Payment> Payments { get; private set; } = null!;
}

public sealed class ReadOnlyApplicationDbContext : BaseApplicationDbContext
{
    
}

public sealed class ReadWriteApplicationDbContext : BaseApplicationDbContext
{
    
}