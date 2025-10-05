using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalTeleportEventHandler : DomainEventHandler<PortalTeleportEvent> {
        readonly PortalRepository _portalRepository;
        readonly PlayerEntity     _playerEntity;

        public PortalTeleportEventHandler(
            PortalRepository portalRepository,
            PlayerEntity     playerEntity
        ) {
            _portalRepository = portalRepository;
            _playerEntity     = playerEntity;
        }

        protected override void Handle(PortalTeleportEvent domainEvent) {
            var portalId       = domainEvent.DestinationID;
            var portal         = _portalRepository.GetByID(portalId);
            var portalPosition = portal.GetPosition();
            _playerEntity.SetPosition(portalPosition);
        }
    }
}