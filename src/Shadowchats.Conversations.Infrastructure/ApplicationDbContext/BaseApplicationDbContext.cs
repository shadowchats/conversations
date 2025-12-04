using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Shadowchats.Conversations.Infrastructure.ApplicationDbContext;

public abstract class BaseApplicationDbContext : DbContext
{
    public Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolationLevel, CancellationToken cancellationToken) => Database.BeginTransactionAsync(isolationLevel, cancellationToken);
}