using System;

namespace TheLostSpirit.Identify
{
    public record struct PortalID : IIdentity, IInteractableID
    {
        public static PortalID New() => new PortalID { Value = Guid.NewGuid() };
        public Guid Value { get; private set; }
    }
}