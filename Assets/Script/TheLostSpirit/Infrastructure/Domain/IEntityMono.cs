using UnityEditor;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain {
    public interface IEntityMono<T> where T : IIdentity {
        public T ID { get; }
        public void Initialize(T id);

        public Transform Transform { get; }
        public void Destroy();
    }
}