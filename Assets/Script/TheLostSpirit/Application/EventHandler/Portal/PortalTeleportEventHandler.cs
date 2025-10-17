using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Portal.Event;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalTeleportEventHandler : DomainEventHandler<PortalTeleportEvent> {
        readonly PortalRepository _portalRepository;

        public PortalTeleportEventHandler(
            PortalRepository portalRepository
        ) {
            _portalRepository = portalRepository;
        }

        protected override void Handle(PortalTeleportEvent domainEvent) {
            var portalId     = domainEvent.Destination;
            var portalEntity = _portalRepository.GetByID(portalId);
            var playerEntity = PlayerEntity.Get();
            playerEntity.SetPosition(portalEntity);
        }
    }
}