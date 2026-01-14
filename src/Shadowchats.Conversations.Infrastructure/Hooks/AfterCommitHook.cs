namespace Shadowchats.Conversations.Infrastructure.Hooks;

public sealed class AfterCommitHook
{
    public AfterCommitHook()
    {
        _actions = [];
    }
    
    public void Register(Func<CancellationToken, Task> action) => _actions.Add(action);

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        foreach (var action in _actions)
            await action(cancellationToken);
    }

    private readonly List<Func<CancellationToken, Task>> _actions;
}