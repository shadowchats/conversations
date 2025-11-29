using Shadowchats.Conversations.Domain.DomainEvents;
using Shadowchats.Conversations.Domain.Entities;

namespace Shadowchats.Conversations.Domain.Aggregates;

// Реализация в условиях Entity Framework
public abstract class BaseAggregate1 : BaseEntity1
{
    protected BaseAggregate1()
    {
        _domainEvents = [];
        DomainEvents = _domainEvents.AsReadOnly();
    }
    
    protected BaseAggregate1(Guid id) : base(id)
    {
        _domainEvents = [];
        DomainEvents = _domainEvents.AsReadOnly();
    }

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    protected void ClearDomainEvents() => _domainEvents.Clear();
    
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    private readonly List<IDomainEvent> _domainEvents;
}

// Реализация в условиях вакуума
public abstract class BaseAggregate : BaseEntity
{
    protected BaseAggregate(Guid id) : base(id)
    {
        _domainEvents = [];
        DomainEvents = _domainEvents.AsReadOnly();
    }

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    protected void ClearDomainEvents() => _domainEvents.Clear();
    
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    private readonly List<IDomainEvent> _domainEvents;
}