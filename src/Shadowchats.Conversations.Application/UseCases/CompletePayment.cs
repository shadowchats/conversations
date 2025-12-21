using MediatR;
using Shadowchats.Conversations.Application.Attributes;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Application.UseCases;

[TracingDecorator]
[LoggingDecorator]
[UnitOfWorkDecorator(DataAccessMode.ReadWrite, TransactionMode.ReadCommitted)]
public sealed record CompletePaymentCommand : IRequest<Unit>
{
    public required Guid PaymentId { get; init; }
}

public sealed class CompletePaymentHandler : IRequestHandler<CompletePaymentCommand, Unit>
{
    public CompletePaymentHandler(IPaymentRepository paymentRepository, IPersistenceContext persistenceContext)
    {
        _paymentRepository = paymentRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<Unit> Handle(CompletePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.Find(p => p.Id == request.PaymentId, cancellationToken);
        if (payment == null)
            throw new InvariantViolationException("Payment does not exist.");
        
        payment.Complete();
        
        await _persistenceContext.SaveChanges(cancellationToken);

        return Unit.Value;
    }
    
    private readonly IPaymentRepository _paymentRepository;
    
    private readonly IPersistenceContext _persistenceContext;
}