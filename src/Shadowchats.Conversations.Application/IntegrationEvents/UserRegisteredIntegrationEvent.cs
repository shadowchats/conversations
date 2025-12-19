namespace Shadowchats.Conversations.Application.IntegrationEvents;

public sealed record UserRegisteredIntegrationEvent : IIntegrationEvent
{
    private UserRegisteredIntegrationEvent()
    {
        UserId = Guid.Empty;
    }

    public UserRegisteredIntegrationEvent(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; private set; }
    
    string IIntegrationEvent.EventType => EventType;
    
    public const string EventType = "UserRegistered";
}

/*public sealed class UserRegisteredIntegrationEventHandler 
    : INotificationHandler<UserRegisteredIntegrationEvent>
{
    private readonly IBuyerRepository _buyerRepository;
    private readonly IPersistenceContext _persistenceContext;
    private readonly ILogger<UserRegisteredIntegrationEventHandler> _logger;

    public UserRegisteredIntegrationEventHandler(
        IBuyerRepository buyerRepository,
        IPersistenceContext persistenceContext,
        ILogger<UserRegisteredIntegrationEventHandler> logger)
    {
        _buyerRepository = buyerRepository;
        _persistenceContext = persistenceContext;
        _logger = logger;
    }

    public async Task Handle(
        UserRegisteredIntegrationEvent evt, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Processing UserRegisteredIntegrationEvent: UserId={UserId}, Email={Email}",
            evt.UserId, evt.Email);

        // Проверяем идемпотентность
        var existingBuyer = await _buyerRepository.GetByUserId(evt.UserId, cancellationToken);
        if (existingBuyer != null)
        {
            _logger.LogWarning("Buyer for UserId={UserId} already exists. Skipping.", evt.UserId);
            return;
        }

        // Создаём Buyer для нового пользователя
        var buyer = Buyer.Create(Guid.NewGuid(), evt.UserId, evt.DisplayName);

        await _buyerRepository.Add(buyer, cancellationToken);
        await _persistenceContext.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Created Buyer {BuyerId} for User {UserId}",
            buyer.Id, evt.UserId);
    }
}*/