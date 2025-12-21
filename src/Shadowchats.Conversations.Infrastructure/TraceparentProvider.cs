using System.Diagnostics;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Infrastructure;

public sealed class TraceparentProvider : ITraceparentProvider
{
    public string Traceparent => Activity.Current?.Id ?? throw new BugException();
}