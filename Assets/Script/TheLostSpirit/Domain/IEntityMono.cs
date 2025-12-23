using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain
{
    public interface IEntityMono<T> : IEntityMono where T : IRuntimeID
    {
        public new T ID { get; }

        public void Initialize(T id);
    }

    public interface IEntityMono
    {
        IRuntimeID ID { get; }
        IReadOnlyTransform ReadOnlyTransform { get; }
        public void Destroy();
    }
}