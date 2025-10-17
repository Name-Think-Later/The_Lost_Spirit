using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Domain {
    public interface IEntityMono<T> where T : IIdentity {
        public T ID { get; }
        public void Initialize(T id);

        public Transform Transform { get; }
        public void Destroy();
    }
}