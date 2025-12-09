using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Domain.Skill.Anchor;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.EntityMono
{
    public class AnchorMono : MonoBehaviour, IAnchorMono
    {
        IEventBus _eventBus;

        public Transform Transform => transform;

        public AnchorID ID { get; private set; }
        public IReadOnlyTransform ReadOnlyTransform { get; private set; }

        public void Initialize(AnchorID id) {
            ID        = id;
            _eventBus = AppScope.EventBus;

            ReadOnlyTransform = transform.ToReadOnly();
        }

        public void SetPosition(Vector2 position) {
            transform.position = position;
        }


        public void Destroy() {
            Destroy(gameObject);
        }
    }
}