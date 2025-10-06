using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Infrastructure.EventDriven;
using UnityEngine;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalInRangeEventHandler : DomainEventHandler<PortalInRangeEvent> {
        readonly PortalRepository       _portalRepository;
        readonly InteractableRepository _interactableRepository;
        readonly PlayerEntity           _playerEntity;

        public PortalInRangeEventHandler(
            PortalRepository       portalRepository,
            InteractableRepository interactableRepository,
            PlayerEntity           playerEntity
        ) {
            _portalRepository       = portalRepository;
            _interactableRepository = interactableRepository;
            _playerEntity           = playerEntity;
        }

        protected override void Handle(PortalInRangeEvent domainEvent) {
            var portalId     = domainEvent.ID;
            var portalEntity = _portalRepository.GetByID(portalId);

            _interactableRepository.Add(portalEntity);

            var nearest = _interactableRepository.GetNearest(_playerEntity);

            _playerEntity.InteractableTarget = nearest.ID;
            nearest.InFocus();
        }
    }
}