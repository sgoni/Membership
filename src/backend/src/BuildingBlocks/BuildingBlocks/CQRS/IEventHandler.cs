using MediatR;

namespace BuildingBlocks.CQRS;

// Handler para eventos (notificaciones)
public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
{
}