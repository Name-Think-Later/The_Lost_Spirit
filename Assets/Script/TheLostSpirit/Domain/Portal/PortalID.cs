using System;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Infrastructure.DomainDriven;

namespace TheLostSpirit.Domain.Portal {
    public class PortalID : IEntityID, IInteractableID {
        public Guid Value { get; } = Guid.NewGuid();
    }
}