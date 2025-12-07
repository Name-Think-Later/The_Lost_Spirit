using TheLostSpirit.Domain.Port;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Portal.Event
{
    public record struct PortalDetectedEvent(PortalID ID) : IDomainEvent;
}