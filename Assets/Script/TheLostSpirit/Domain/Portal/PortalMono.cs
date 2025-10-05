using R3;
using R3.Triggers;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Infrastructure;
using UnityEngine;

namespace TheLostSpirit.Domain.Portal {
    public class PortalMono : MonoBehaviour, IPortalMono {
        [SerializeField]
        Collider2D _collider;

        public PortalID ID { get; private set; }
        public Transform Transform => transform;


        public void Initialize(PortalID id) {
            ID = id;

            BindTriggerEnter();

            BindTriggerExit();
        }

        void BindTriggerEnter() {
            var triggerEnter =
                _collider.OnTriggerEnter2DAsObservable();

            triggerEnter
                .Subscribe(_ => {
                    var portalInRange = new PortalInRangeEvent(ID);
                    AppScope.EventBus.Publish(portalInRange);
                })
                .AddTo(this);
        }

        void BindTriggerExit() {
            var triggerExit
                = _collider.OnTriggerExit2DAsObservable();

            triggerExit
                .Subscribe(_ => {
                    var portalOutOfRange = new PortalOutOfRangeEvent(ID);
                    AppScope.EventBus.Publish(portalOutOfRange);
                })
                .AddTo(this);
        }
    }
}