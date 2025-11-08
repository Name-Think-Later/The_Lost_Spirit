using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Portal.Event;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalInteractedEventHandler : DomainEventHandler<PortalInteractedEvent> {
        readonly PortalRepository _portalRepository;

        public PortalInteractedEventHandler(
            PortalRepository portalRepository
        ) {
            _portalRepository = portalRepository;
        }

        protected override void Handle(PortalInteractedEvent domainEvent) {
            var portalId     = domainEvent.Destination;
            var portalEntity = _portalRepository.GetByID(portalId);
            var playerEntity = PlayerEntity.Get();
            playerEntity.SetPosition(portalEntity);
        }
    }
}