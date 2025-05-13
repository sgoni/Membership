using MediatR;

namespace BuildingBlocks.CQRS;

// Interface para eventos (notificaciones)
public interface IEvent : INotification
{
}