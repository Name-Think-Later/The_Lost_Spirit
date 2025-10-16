using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Domain.Room {
    public class RoomMono : MonoBehaviour, IRoomMono {
        public RoomID ID { get; private set; }

        public void Initialize(RoomID id) {
            ID = id;
        }

        public Transform Transform => transform;
        public void Destroy() => Destroy(gameObject);
    }
}