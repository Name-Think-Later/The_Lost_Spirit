using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Portal.Event
{
    public record struct PortalConnectedEvent(PortalID LeftID, PortalID RightID) : IDomainEvent;
}