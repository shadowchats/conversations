using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Infrastructure.Interfaces;

public interface IInfrastructureUnitOfWork : IUnitOfWork
{
    IApplicationDbContext DbContext { get; }
}