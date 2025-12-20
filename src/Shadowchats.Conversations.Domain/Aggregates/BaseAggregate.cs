using MediatR;
using Shadowchats.Conversations.Domain.Entities;

namespace Shadowchats.Conversations.Domain.Aggregates;

public abstract class BaseAggregate : BaseEntity
{
    protected BaseAggregate()
    {
        _domainEvents = [];
        DomainEvents = _domainEvents.AsReadOnly();
    }
    
    protected BaseAggregate(Guid id) : base(id)
    {
        _domainEvents = [];
        DomainEvents = _domainEvents.AsReadOnly();
    }

    protected void AddDomainEvent(IRequest<Unit> domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
    
    public IReadOnlyList<IRequest<Unit>> DomainEvents { get; }

    private readonly List<IRequest<Unit>> _domainEvents;
}