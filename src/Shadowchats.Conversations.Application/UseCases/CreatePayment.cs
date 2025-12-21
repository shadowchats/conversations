using MediatR;
using Shadowchats.Conversations.Application.Attributes;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Application.Mappers;
using Shadowchats.Conversations.Domain.Aggregates;
using Shadowchats.Conversations.Domain.Interfaces;
using Shadowchats.Conversations.Domain.ValueObjects;

namespace Shadowchats.Conversations.Application.UseCases;

[TracingDecorator]
[LoggingDecorator]
[UnitOfWorkDecorator(DataAccessMode.ReadWrite, TransactionMode.ReadCommitted)]
public sealed record CreatePaymentCommand : IRequest<CreatePaymentResponse>
{
    public required Guid OrderId { get; init; }
    
    public required decimal Amount { get; init; }
    
    public required string Currency { get; init; }
    
    public required string PaymentMethod { get; init; }
}

public sealed record CreatePaymentResponse
{
    public required Guid PaymentId { get; init; }
}

public sealed class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
{
    public CreatePaymentHandler(IGuidGenerator guidGenerator, IPaymentRepository paymentRepository, IPersistenceContext persistenceContext)
    {
        _guidGenerator = guidGenerator;
        _paymentRepository = paymentRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<CreatePaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = Payment.Create(
            _guidGenerator,
            request.OrderId,
            Money.Create(request.Amount, EnumsMapper.MapCurrency(request.Currency)),
            EnumsMapper.MapPaymentMethod(request.PaymentMethod)
        );
        await _paymentRepository.Add(payment, cancellationToken);
        await _persistenceContext.SaveChanges(cancellationToken);

        return new CreatePaymentResponse { PaymentId = payment.Id };
    }
    
    private readonly IGuidGenerator _guidGenerator;
    
    private readonly IPaymentRepository _paymentRepository;
    
    private readonly IPersistenceContext _persistenceContext;
}