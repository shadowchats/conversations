namespace Shadowchats.Conversations.Domain.Entities;

// Реализация в условиях Entity Framework
public abstract class BaseEntity1
{
    protected BaseEntity1()
    {
        Id = Guid.Empty;
    }
    
    protected BaseEntity1(Guid id)
    {
        Id = id;
    }

    public sealed override bool Equals(object? obj) =>
        obj is BaseEntity entity && GetType() == entity.GetType() && Id == entity.Id;

    public sealed override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(BaseEntity1? left, BaseEntity1? right) =>
        ReferenceEquals(left, right) || (left is not null && left.Equals(right));

    public static bool operator !=(BaseEntity1? left, BaseEntity1? right) => !(left == right);

    public Guid Id { get; private init; }
}

// Реализация в условиях вакуума
public abstract class BaseEntity
{
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

    public Guid Id { get; }
}