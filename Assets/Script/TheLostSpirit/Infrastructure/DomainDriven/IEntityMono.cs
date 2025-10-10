using UnityEngine;

namespace TheLostSpirit.Infrastructure.DomainDriven {
    public interface IEntityMono<T> where T : IEntityID {
        public T ID { get; }
        public void Initialize(T id);

        public Transform Transform { get; }
    }
}