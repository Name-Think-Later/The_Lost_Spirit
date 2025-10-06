using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.IDentify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.DomainDriven;
using TheLostSpirit.Infrastructure.EventDriven;
using UnityEngine;

namespace TheLostSpirit.Domain.Portal {
    public class PortalEntity : IEntity<PortalID>, IInteractable {
        readonly EventBus    _eventBus;
        readonly Portal      _portal;
        readonly IPortalMono _portalMono;

        public PortalEntity(PortalID id, IPortalMono portalMono) {
            _eventBus = AppScope.EventBus;

            ID = id;

            _portal = new Portal();

            _portalMono = portalMono;
            _portalMono.Initialize(ID);
        }

        public PortalID ID { get; }
        IInteractableID IEntity<IInteractableID>.ID => ID;
        
        public Vector2 Position => _portalMono.Transform.position;

        public void LinkTo(PortalID destinationID) {
            _portal.DestinationID = destinationID;
        }

        public void InFocus() {
            var inFocus = new PortalInFocusEvent(ID);
            _eventBus.Publish(inFocus);
        }

        public void OutOfFocus() {
            var outOfFocus = new PortalOutOfFocusEvent(ID);
            _eventBus.Publish(outOfFocus);
        }

        public void Interacted() {
            var destinationID = _portal.DestinationID;

            if (destinationID == null) return;
            var portalTeleport = new PortalTeleportEvent(destinationID);
            _eventBus.Publish(portalTeleport);
        }
    }
}