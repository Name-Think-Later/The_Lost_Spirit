using TheLostSpirit.IDentify;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Portal {
    public record PortalInFocusEvent(PortalID ID) : DomainEvent;
}