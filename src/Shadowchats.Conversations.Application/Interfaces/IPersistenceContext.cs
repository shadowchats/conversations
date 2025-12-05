namespace Shadowchats.Conversations.Application.Interfaces;

public interface IPersistenceContext
{
    Task SaveChanges(CancellationToken cancellationToken);
}