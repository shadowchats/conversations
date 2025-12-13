using MediatR;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Aggregates;
using Shadowchats.Conversations.Domain.ValueObjects;

namespace Shadowchats.Conversations.Application.UseCases;

public sealed record CreatePaymentCommand : IRequest<CreatePaymentResponse>
{
    public required Guid PaymentId { get; init; }
    
    public required Guid OrderId { get; init; }
    
    public required decimal Amount { get; init; }
    
    public required byte Currency { get; init; }
    
    public required byte PaymentMethod { get; init; }
}

public sealed record CreatePaymentResponse
{
    public required Guid PaymentId { get; init; }
}

public sealed class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
{
    public CreatePaymentHandler(IPaymentRepository paymentRepository, IPersistenceContext persistenceContext)
    {
        _paymentRepository = paymentRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<CreatePaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = Payment.Create(
            request.PaymentId,
            request.OrderId,
            Money.Create(request.Amount, (Domain.Enums.Currency)request.Currency),
            (Domain.Enums.PaymentMethod)request.PaymentMethod
        );
        await _paymentRepository.Add(payment, cancellationToken);
        await _persistenceContext.SaveChanges(cancellationToken);

        return new CreatePaymentResponse { PaymentId = payment.Id };
    }
    
    private readonly IPaymentRepository _paymentRepository;
    
    private readonly IPersistenceContext _persistenceContext;
}