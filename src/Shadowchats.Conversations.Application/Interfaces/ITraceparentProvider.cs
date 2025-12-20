namespace Shadowchats.Conversations.Application.Interfaces;

public interface ITraceparentProvider
{
    string Traceparent { get; }
}