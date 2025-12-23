using System.Collections.Generic;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.EntityMono
{
    public class RoomMono : MonoBehaviour, IRoomMono
    {
        IEventBus _eventBus;

        public Transform Transform => transform;

        public RoomID ID { get; private set; }
        IRuntimeID IEntityMono.ID => ID;
        public IReadOnlyTransform ReadOnlyTransform { get; private set; }

        public void Initialize(RoomID id) {
            ID        = id;
            _eventBus = AppScope.EventBus;

            ReadOnlyTransform = transform.ToReadOnly();
        }


        public void Destroy() {
            Destroy(gameObject);
        }
    }
}