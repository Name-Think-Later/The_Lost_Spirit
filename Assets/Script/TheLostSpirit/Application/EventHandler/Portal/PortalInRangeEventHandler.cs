using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalInRangeEventHandler : DomainEventHandler<PortalInRangeEvent> {
        readonly PortalRepository       _portalRepository;
        readonly InteractableRepository _interactableRepository;

        public PortalInRangeEventHandler(
            PortalRepository       portalRepository,
            InteractableRepository interactableRepository
        ) {
            _portalRepository       = portalRepository;
            _interactableRepository = interactableRepository;
        }

        protected override void Handle(PortalInRangeEvent domainEvent) {
            var portalId     = domainEvent.PortalID;
            var portalEntity = _portalRepository.GetByID(portalId);

            _interactableRepository.Add(portalEntity);
        }
    }
}