using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain
{
    public interface IEntity<out T> where T : IRuntimeID
    {
        T ID { get; }
    }
}