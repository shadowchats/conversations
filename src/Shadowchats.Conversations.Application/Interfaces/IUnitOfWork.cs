namespace Shadowchats.Conversations.Application.Interfaces;

public interface IUnitOfWork
{
    Task Begin(Type requestType, CancellationToken cancellationToken);

    Task Commit(CancellationToken cancellationToken);

    Task Rollback(CancellationToken cancellationToken);
}