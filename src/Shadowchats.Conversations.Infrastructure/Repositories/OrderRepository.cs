using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    public OrderRepository(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private readonly UnitOfWork _unitOfWork;
}