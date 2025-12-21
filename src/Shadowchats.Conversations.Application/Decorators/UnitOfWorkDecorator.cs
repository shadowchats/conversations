using MediatR;
using Shadowchats.Conversations.Application.Attributes;
using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Application.Decorators;

public class UnitOfWorkDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public UnitOfWorkDecorator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var attribute = AttributeCache.Get<UnitOfWorkDecoratorAttribute>(typeof(TRequest));
        if (attribute == null)
            return await next(cancellationToken);
        
        await _unitOfWork.Begin(attribute.Options, cancellationToken);

        try
        {
            var response = await next(cancellationToken);

            await _unitOfWork.Commit(cancellationToken);
            
            return response;
        }
        catch
        {
            await _unitOfWork.Rollback(cancellationToken);
            
            throw;
        }
    }

    private readonly IUnitOfWork _unitOfWork;
}