using System;
using System.Diagnostics.Contracts;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Portal
{
    public class PortalEntity : IEntity<PortalID>, IDisposable
    {
        readonly Portal      _portal;
        readonly IPortalMono _portalMono;
        readonly IEventBus   _eventBus;

        public PortalID ID { get; }

        public PortalID AssociatedPortal => _portal.AssociatedPortal;
        public bool CanInteract => HasAssociated && _portal.IsEnable;
        public bool HasAssociated => _portal.AssociatedPortal != null;

        public IReadOnlyTransform ReadOnlyTransform => _portalMono.ReadOnlyTransform;


        public PortalEntity(PortalID id, IPortalMono mono) {
            ID = id;

            _eventBus = AppScope.EventBus;

            _portal = new Portal();

            _portalMono = mono;
            _portalMono.Initialize(ID);
        }

        public void Associate(PortalID other) {
            _portal.AssociatedPortal = other;

            var portalConnected = new PortalConnectedEvent(ID, other);
            _eventBus.Publish(portalConnected);
        }

        public void Interact() {
            Contract.Assert(HasAssociated, "Can't interacted without associated portal");

            var destination      = _portal.AssociatedPortal;
            var portalInteracted = new PortalInteractedEvent(destination);

            _eventBus.Publish(portalInteracted);
        }

        public void Dispose() {
            _portalMono.Destroy();
        }
    }
}