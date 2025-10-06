using TheLostSpirit.IDentify;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Portal {
    public record PortalOutOfFocusEvent(PortalID ID) : DomainEvent;
}