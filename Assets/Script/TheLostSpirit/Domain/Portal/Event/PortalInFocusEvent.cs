using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Portal.Event {
    public record PortalInFocusEvent(PortalID ID) : DomainEvent;
}