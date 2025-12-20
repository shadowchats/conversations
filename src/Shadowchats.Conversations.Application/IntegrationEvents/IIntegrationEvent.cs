using MediatR;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

public interface IIntegrationEvent : IRequest<Unit>
{
    string EventType { get; }
}