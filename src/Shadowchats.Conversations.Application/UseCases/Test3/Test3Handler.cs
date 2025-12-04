using MediatR;

namespace Shadowchats.Conversations.Application.UseCases.Test3;

public class Test3Handler : IRequestHandler<Test3Command, Unit>
{
    public Task<Unit> Handle(Test3Command request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}