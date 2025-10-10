using System;
using TheLostSpirit.Infrastructure.DomainDriven;

namespace TheLostSpirit.Identify {
    public class PortalID : IEntityID, IInteractableID {
        public Guid Value { get; } = Guid.NewGuid();
    }
}