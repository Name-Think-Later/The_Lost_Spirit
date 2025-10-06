using TheLostSpirit.IDentify;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Portal.Event {
    public record PortalOutOfRangeEvent(PortalID ID) : DomainEvent;
}