using MediatR;
using Microsoft.Extensions.Logging;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Application.Decorators;

public sealed class LoggingDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public LoggingDecorator(ILogger<LoggingDecorator<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _requestName = typeof(TRequest).Name;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Stage: {Stage}; RequestName: {RequestName}; RequestPayload: {@RequestPayload}.",
                "Start", _requestName, request);

        try
        {
            var response = await next(cancellationToken);

            if (_logger.IsEnabled(LogLevel.Information)) 
                _logger.LogInformation("Stage: {Stage}. Response: {@Response}.", "Success", response);

            return response;
        }
        catch (BaseException expectedException) when (expectedException is not BugException)
        {
            if (_logger.IsEnabled(LogLevel.Information)) 
                _logger.LogInformation(expectedException, "Stage: {Stage}.", "ExpectedFailure");

            throw;
        }
        catch (Exception unexpectedException)
        {
            if (_logger.IsEnabled(LogLevel.Error)) 
                _logger.LogError(unexpectedException, "Stage: {Stage}.", "UnexpectedFailure");

            throw;
        }
    }

    private readonly ILogger<LoggingDecorator<TRequest, TResponse>> _logger;

    private readonly string _requestName;
}