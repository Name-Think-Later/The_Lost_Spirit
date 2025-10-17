using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Portal.Event;

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
            var portalID = domainEvent.ID;

            var entity = _portalRepository.GetByID(portalID);

            if (!entity.CanInteract) return;
            _interactableRepository.Add(entity);

            var playerEntity = PlayerEntity.Get();
            var nearest      = _interactableRepository.GetNearest(playerEntity);
            playerEntity.InteractableTarget = nearest.ID;
            nearest.InFocus();
        }
    }
}