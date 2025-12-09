using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain
{
    public interface IEntityMono<T> : IEntity<T> where T : IRuntimeID
    {
        IReadOnlyTransform ReadOnlyTransform { get; }
        public void Initialize(T id);

        public void Destroy();
    }
}