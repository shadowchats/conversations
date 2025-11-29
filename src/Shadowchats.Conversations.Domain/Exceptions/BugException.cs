namespace Shadowchats.Conversations.Domain.Exceptions;

public sealed class BugException : BaseException
{
    public BugException(string message) : base(message) { }
}