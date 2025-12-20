namespace Shadowchats.Conversations.Domain.Entities;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.Empty;
    }
    
    protected BaseEntity(Guid id)
    {
        Id = id;
    }

    public sealed override bool Equals(object? obj) =>
        obj is BaseEntity entity && GetType() == entity.GetType() && Id == entity.Id;

    public sealed override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(BaseEntity? left, BaseEntity? right) =>
        ReferenceEquals(left, right) || (left is not null && left.Equals(right));

    public static bool operator !=(BaseEntity? left, BaseEntity? right) => !(left == right);

    public Guid Id { get; private init; }
}