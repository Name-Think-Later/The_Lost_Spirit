using R3;
using R3.Triggers;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.EventDriven;
using UnityEngine;

namespace TheLostSpirit.Domain.Portal
{
    public class PortalMono : MonoBehaviour, IPortalMono
    {
        [SerializeField]
        Collider2D _collider;

        EventBus _eventBus;

        public PortalID ID { get; private set; }
        public Transform Transform => transform;

        public void Initialize(PortalID id) {
            ID = id;

            _eventBus = AppScope.EventBus;

            BindTriggerEnter();
            BindTriggerExit();
        }

        void BindTriggerEnter() {
            var triggerEnter =
                _collider.OnTriggerEnter2DAsObservable();

            triggerEnter
                .Subscribe(_ => {
                    var portalTriggerEntered = new PortalTriggerEnteredEvent(ID);
                    _eventBus.Publish(portalTriggerEntered);
                })
                .AddTo(this);
        }

        void BindTriggerExit() {
            var triggerExit
                = _collider.OnTriggerExit2DAsObservable();

            triggerExit
                .Subscribe(_ => {
                    var portalTriggerExited = new PortalTriggerExitedEvent(ID);
                    _eventBus.Publish(portalTriggerExited);
                })
                .AddTo(this);
        }

        public void Destroy() => Destroy(gameObject);
    }
}