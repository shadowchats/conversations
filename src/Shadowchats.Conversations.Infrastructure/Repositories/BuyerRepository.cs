using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Aggregates;

namespace Shadowchats.Conversations.Infrastructure.Repositories;

public sealed class BuyerRepository : IBuyerRepository
{
    public BuyerRepository(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task Add(Buyer buyer, CancellationToken cancellationToken) =>
        _unitOfWork.DbContext.Buyers.AddAsync(buyer, cancellationToken).AsTask();

    private readonly UnitOfWork _unitOfWork;
}