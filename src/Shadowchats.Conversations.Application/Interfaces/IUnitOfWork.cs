using Shadowchats.Conversations.Application.Common;

namespace Shadowchats.Conversations.Application.Interfaces;

public interface IUnitOfWork
{
    Task Begin(UnitOfWorkOptions applicationOptions, CancellationToken cancellationToken);

    Task Commit(CancellationToken cancellationToken);

    Task Rollback(CancellationToken cancellationToken);
}