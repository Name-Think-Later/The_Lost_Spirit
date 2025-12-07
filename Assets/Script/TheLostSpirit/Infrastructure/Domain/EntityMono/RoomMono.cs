using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.EntityMono
{
    public class RoomMono : MonoBehaviour, IRoomMono
    {
        IEventBus _eventBus;

        public RoomID ID { get; private set; }

        public void Initialize(RoomID id) {
            ID        = id;
            _eventBus = AppScope.EventBus;
        }

        public IReadOnlyTransform ReadOnlyTransform { get; }

        public Transform Transform => transform;
        public void Destroy() => Destroy(gameObject);
    }
}