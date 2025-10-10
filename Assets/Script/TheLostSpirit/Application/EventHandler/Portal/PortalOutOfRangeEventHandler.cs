using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Infrastructure.EventDriven;
using UnityEngine;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalOutOfRangeEventHandler : DomainEventHandler<PortalOutOfRangeEvent> {
        readonly PortalRepository       _portalRepository;
        readonly InteractableRepository _interactableRepository;
        readonly PlayerEntity           _playerEntity;

        public PortalOutOfRangeEventHandler(
            PortalRepository       portalRepository,
            InteractableRepository interactableRepository,
            PlayerEntity           playerEntity
        ) {
            _portalRepository       = portalRepository;
            _interactableRepository = interactableRepository;
            _playerEntity           = playerEntity;
        }

        protected override void Handle(PortalOutOfRangeEvent domainEvent) {
            var portalId = domainEvent.ID;

            var target = _interactableRepository.GetByID(portalId);
            target.OutOfFocus();

            _interactableRepository.Remove(portalId);

            var nearest = _interactableRepository.GetNearest(_playerEntity);

            if (nearest == null) {
                _playerEntity.InteractableTarget = null;
            }
            else {
                _playerEntity.InteractableTarget = nearest.ID;
                nearest.InFocus();
            }
        }
    }
}