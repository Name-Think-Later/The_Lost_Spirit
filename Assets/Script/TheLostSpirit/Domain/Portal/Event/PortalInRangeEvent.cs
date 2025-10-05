using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Portal.Event {
    public record PortalInRangeEvent(PortalID PortalID) : DomainEvent;
}