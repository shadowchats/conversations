namespace Shadowchats.Conversations.Application.Interfaces;

public interface IPersistenceContext
{
    Task SaveChanges(CancellationToken cancellationToken);

    void AfterCommit(Func<CancellationToken, Task> action);
}