using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.EntityMono
{
    public class PortalMono : MonoBehaviour, IPortalMono, IInteractableComponent
    {
        [SerializeField]
        Collider2D _collider;

        IEventBus _eventBus;


        public void Interacted() {
            var detectedEvent = new PortalInteractedEvent(ID);
            _eventBus.Publish(detectedEvent);
        }

        public void Detected() {
            var detectedEvent = new PortalDetectedEvent(ID);
            _eventBus.Publish(detectedEvent);
        }

        public void Undetected() {
            var undetectedEvent = new PortalUndetectedEvent(ID);
            _eventBus.Publish(undetectedEvent);
        }

        public PortalID ID { get; private set; }

        public IReadOnlyTransform ReadOnlyTransform { get; private set; }

        public void Initialize(PortalID id) {
            ID        = id;
            _eventBus = AppScope.EventBus;

            ReadOnlyTransform = transform.ToReadOnly();
        }

        public void Destroy() {
            Destroy(gameObject);
        }
    }
}