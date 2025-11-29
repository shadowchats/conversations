namespace Shadowchats.Conversations.Domain.Exceptions;

public sealed class InvariantViolationException : BaseException
{
    public InvariantViolationException(string message) : base(message) { }
}