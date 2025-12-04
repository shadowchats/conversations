using MediatR;

namespace Shadowchats.Conversations.Application.UseCases.Test2;

public class Test2Handler : IRequestHandler<Test2Query, Test2Response>
{
    public Task<Test2Response> Handle(Test2Query request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}