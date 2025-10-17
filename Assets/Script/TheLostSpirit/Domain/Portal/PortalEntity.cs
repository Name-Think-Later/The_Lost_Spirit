using System;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.EventDriven;
using UnityEngine;

namespace TheLostSpirit.Domain.Portal {
    public class PortalEntity : IEntity<PortalID>, IInteractable, IDisposable {
        readonly Portal      _portal;
        readonly IPortalMono _portalMono;
        readonly EventBus    _eventBus;
        public PortalID ID { get; }
        IInteractableID IEntity<IInteractableID>.ID => ID;

        public bool CanInteract => _portal.HasAssociated && _portal.IsEnable;

        public bool HasAssociate => _portal.HasAssociated;

        public ReadOnlyTransform ReadOnlyTransform { get; private set; }

        public Vector2 Position => _portalMono.Transform.position;


        public PortalEntity(PortalID id, IPortalMono mono) {
            ID = id;

            _eventBus = AppScope.EventBus;

            _portal = new Portal();

            _portalMono = mono;
            _portalMono.Initialize(ID);

            ReadOnlyTransform = new ReadOnlyTransform(_portalMono.Transform);
        }


        public void Associate(PortalID other) {
            _portal.AssociatedPortal = other;

            var portalConnected = new PortalConnectedEvent(ID, other);
            _eventBus.Publish(portalConnected);
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
            var destination    = _portal.AssociatedPortal;
            var portalTeleport = new PortalTeleportEvent(destination);

            _eventBus.Publish(portalTeleport);
        }

        public void Dispose() {
            _portalMono.Destroy();
        }
    }
}