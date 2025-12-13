using MediatR;
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

    protected void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
    
    public IReadOnlyList<INotification> DomainEvents { get; }

    private readonly List<INotification> _domainEvents;
}

// Реализация в условиях вакуума
public abstract class BaseAggregate : BaseEntity
{
    protected BaseAggregate(Guid id) : base(id)
    {
        _domainEvents = [];
        DomainEvents = _domainEvents.AsReadOnly();
    }

    protected void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
    
    public IReadOnlyList<INotification> DomainEvents { get; }

    private readonly List<INotification> _domainEvents;
}