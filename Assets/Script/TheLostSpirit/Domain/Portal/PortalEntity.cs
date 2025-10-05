using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.DomainDriven;
using TheLostSpirit.Infrastructure.EventDriven;
using UnityEngine;

namespace TheLostSpirit.Domain.Portal {
    public class PortalEntity : IEntity<PortalID>, IInteractable {
        readonly EventBus    _eventBus;
        readonly Portal      _portal;
        readonly IPortalMono _portalMono;

        public PortalEntity(IPortalMono portalMono) {
            _eventBus = AppScope.EventBus;

            ID = new PortalID();

            _portal = new Portal();

            _portalMono = portalMono;
            _portalMono.Initialize(ID);
        }

        public PortalID ID { get; }
        IInteractableID IEntity<IInteractableID>.ID => ID;

        public void LinkTo(PortalID destinationID) {
            _portal.DestinationID = destinationID;
        }

        public void Interacted() {
            var destinationID = _portal.DestinationID;

            if (destinationID == null) return;
            Debug.Log(destinationID);
            var portalTeleport = new PortalTeleportEvent(destinationID);
            _eventBus.Publish(portalTeleport);
        }

        public Vector2 GetPosition() {
            return _portalMono.Transform.position;
        }
    }
}