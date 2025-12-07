using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain
{
    public interface IEntityMono<T> : IEntity<T> where T : IRuntimeID
    {
        public void Initialize(T id);

        IReadOnlyTransform ReadOnlyTransform { get; }

        public void Destroy();
    }
}