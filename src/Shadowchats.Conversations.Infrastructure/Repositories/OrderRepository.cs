using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Infrastructure.Interfaces;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    public OrderRepository(IInfrastructureUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private readonly IInfrastructureUnitOfWork _unitOfWork;
}