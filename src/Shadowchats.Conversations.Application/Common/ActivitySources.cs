using System.Diagnostics;

namespace Shadowchats.Conversations.Application.Common;

public static class ActivitySources
{
    public static readonly ActivitySource Application = new("Shadowchats.Conversations.Application");
}