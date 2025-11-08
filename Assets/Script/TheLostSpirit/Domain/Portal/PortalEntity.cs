using System;
using System.Diagnostics.Contracts;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.EventDriven;
using UnityEngine;

namespace TheLostSpirit.Domain.Portal
{
    public class PortalEntity : IEntity<PortalID>, IInteractable, IDisposable
    {
        readonly Portal      _portal;
        readonly IPortalMono _portalMono;
        readonly EventBus    _eventBus;
        public PortalID ID { get; }
        IInteractableID IEntity<IInteractableID>.ID => ID;

        public bool CanInteract => HasAssociated && _portal.IsEnable;

        public bool HasAssociated => _portal.AssociatedPortal.HasValue;

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
            var inFocused = new PortalInFocusedEvent(ID);
            _eventBus.Publish(inFocused);
        }

        public void OutOfFocus() {
            var outOfFocused = new PortalOutOfFocusedEvent(ID);

            _eventBus.Publish(outOfFocused);
        }

        public void Interact() {
            Contract.Assert(_portal.AssociatedPortal.HasValue, "Can't interacted without associated portal");

            var destination      = _portal.AssociatedPortal.Value;
            var portalInteracted = new PortalInteractedEvent(destination);

            _eventBus.Publish(portalInteracted);
        }

        public void Dispose() {
            _portalMono.Destroy();
        }
    }
}