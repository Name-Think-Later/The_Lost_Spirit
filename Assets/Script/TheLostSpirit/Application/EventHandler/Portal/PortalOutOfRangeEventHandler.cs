using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Portal.Event;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalOutOfRangeEventHandler : DomainEventHandler<PortalOutOfRangeEvent> {
        readonly InteractableRepository _interactableRepository;

        public PortalOutOfRangeEventHandler(
            InteractableRepository interactableRepository
        ) {
            _interactableRepository = interactableRepository;
        }

        protected override void Handle(PortalOutOfRangeEvent domainEvent) {
            var portalID = domainEvent.ID;

            if (!_interactableRepository.HasID(portalID)) return;
            var target = _interactableRepository.GetByID(portalID);
            target.OutOfFocus();
            _interactableRepository.Remove(portalID);

            var playerEntity = PlayerEntity.Get();
            var nearest      = _interactableRepository.GetNearest(playerEntity);

            if (nearest == null) {
                playerEntity.InteractableTarget = null;
            }
            else {
                playerEntity.InteractableTarget = nearest.ID;
                nearest.InFocus();
            }
        }
    }
}