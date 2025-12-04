using MediatR;

namespace Shadowchats.Conversations.Application.UseCases.Test1;

public class Test1Handler : IRequestHandler<Test1Command, Test1Response>
{
    public Task<Test1Response> Handle(Test1Command request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}