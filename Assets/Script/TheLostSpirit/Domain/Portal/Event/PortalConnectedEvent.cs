using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Portal.Event {
    public record struct PortalConnectedEvent(PortalID LeftID, PortalID RightID) : IDomainEvent;
}