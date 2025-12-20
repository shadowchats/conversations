namespace Shadowchats.Conversations.Domain.Aggregates;

public sealed class Buyer : BaseAggregate
{
    private Buyer() { }

    public Buyer(Guid id) : base(id) { }
}