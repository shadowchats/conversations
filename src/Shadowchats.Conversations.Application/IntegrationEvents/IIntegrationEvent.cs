using MediatR;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

public interface IIntegrationEvent : INotification
{
    string EventType { get; }
}