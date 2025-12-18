using System.Diagnostics;
using MediatR;
using Shadowchats.Conversations.Application.Common;

namespace Shadowchats.Conversations.Application.Decorators;

public sealed class TracingDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var activity = ActivitySources.Application.StartActivity(typeof(TRequest).Name);
        activity?.SetTag("request.type", typeof(TRequest).Name);

        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            activity?.SetStatus(ActivityStatusCode.Error);
            throw;
        }
    }
}