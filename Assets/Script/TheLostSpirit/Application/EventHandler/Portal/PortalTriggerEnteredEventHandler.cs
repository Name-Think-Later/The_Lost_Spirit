using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Portal.Event;

namespace TheLostSpirit.Application.EventHandler.Portal
{
    public class PortalTriggerEnteredEventHandler : DomainEventHandler<PortalTriggerEnteredEvent>
    {
        readonly PortalRepository       _portalRepository;
        readonly InteractableRepository _interactableRepository;

        public PortalTriggerEnteredEventHandler(
            PortalRepository       portalRepository,
            InteractableRepository interactableRepository
        ) {
            _portalRepository       = portalRepository;
            _interactableRepository = interactableRepository;
        }

        protected override void Handle(PortalTriggerEnteredEvent domainEvent) {
            var portalID = domainEvent.ID;

            var portalEntity = _portalRepository.GetByID(portalID);

            if (!portalEntity.CanInteract) return;
            _interactableRepository.Save(portalEntity);

            var playerEntity = PlayerEntity.Get();
            var nearest      = _interactableRepository.GetNearest(playerEntity);
            playerEntity.InteractableTarget = nearest.ID;
            nearest.InFocus();
        }
    }
}