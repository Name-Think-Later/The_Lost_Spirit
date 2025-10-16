using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Portal.Event {
    public record struct PortalTeleportEvent(PortalID Destination) : IDomainEvent;
}