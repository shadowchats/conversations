using MediatR;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Enums;
using Shadowchats.Conversations.Domain.Interfaces;

namespace Shadowchats.Conversations.Application.Jobs;

public sealed record CancelUnpaidOrdersJob : IRequest<Unit>;

public sealed class CancelUnpaidOrdersHandler : IRequestHandler<CancelUnpaidOrdersJob, Unit>
{
    public CancelUnpaidOrdersHandler(IDateTimeProvider dateTimeProvider, IOrderRepository orderRepository, IPersistenceContext persistenceContext)
    {
        _dateTimeProvider = dateTimeProvider;
        _orderRepository = orderRepository;
        _persistenceContext = persistenceContext;
    }

    public async Task<Unit> Handle(CancelUnpaidOrdersJob request, CancellationToken cancellationToken)
    {
        var threshold = _dateTimeProvider.UtcNow - TimeSpan.FromHours(24);
        var unpaidOrders = await _orderRepository.FindAll(o => o.Status == OrderStatus.Created && o.CreatedAt <= threshold, cancellationToken);
        
        foreach (var order in unpaidOrders)
            order.Cancel();

        await _persistenceContext.SaveChanges(cancellationToken);
        
        return Unit.Value;
    }
    
    private readonly IDateTimeProvider _dateTimeProvider;
    
    private readonly IOrderRepository _orderRepository;
    
    private readonly IPersistenceContext _persistenceContext;
}