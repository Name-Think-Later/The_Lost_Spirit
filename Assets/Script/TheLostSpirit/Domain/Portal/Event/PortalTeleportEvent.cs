using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Portal.Event {
    public record PortalTeleportEvent(PortalID DestinationID) : DomainEvent;
}