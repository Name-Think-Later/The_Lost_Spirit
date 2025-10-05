using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalOutOfRangeEventHandler : DomainEventHandler<PortalOutOfRangeEvent> {
        readonly PortalRepository       _portalRepository;
        readonly InteractableRepository _interactableRepository;

        public PortalOutOfRangeEventHandler(
            PortalRepository       portalRepository,
            InteractableRepository interactableRepository
        ) {
            _portalRepository       = portalRepository;
            _interactableRepository = interactableRepository;
        }

        protected override void Handle(PortalOutOfRangeEvent domainEvent) {
            var portalId = domainEvent.ID;
            _interactableRepository.Remove(portalId);
        }
    }
}