namespace Shadowchats.Conversations.Domain.Exceptions;

public sealed class BugException : BaseException
{
    public BugException() : base(DefaultMessage) { }
    
    private const string DefaultMessage = "Bug detected: debug and check logs.";
}