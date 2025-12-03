using MediatR;

namespace Shadowchats.Conversations.Application.Decorators;

public class UnitOfWorkDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public UnitOfWorkDecorator(IUnitOfWork unitOfWork, IServiceProvider services, IMessageHandler<TMessage, TResult> decorated)
    {
        _unitOfWork = unitOfWork;
        _services = services;
        _decorated = decorated;
    }

    public async Task<TResult> Handle(TMessage message)
    {
        var (dbContextKeyType, transactionMode) = message switch
        {
            IQuery<TResult>   => (typeof(AuthenticationDbContextReadOnly), IUnitOfWork.TransactionMode.None),
            ICommand<TResult> => (typeof(AuthenticationDbContextReadWrite), IUnitOfWork.TransactionMode.WithReadCommitted),
            _ => throw new BugException("Unhandled message type.")
        };

        await _unitOfWork.Begin((IAuthenticationDbContext)_services.GetRequiredService(dbContextKeyType),
            transactionMode);

        try
        {
            var result = await _decorated.Handle(message);

            await _unitOfWork.End(IUnitOfWork.Outcome.Success);
            
            return result;
        }
        catch
        {
            await _unitOfWork.End(IUnitOfWork.Outcome.Failure);
            
            throw;
        }
    }

    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IServiceProvider _services;
}