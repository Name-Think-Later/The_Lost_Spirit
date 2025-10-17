using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain {
    public interface IEntity<out T> where T : IIdentity {
        T ID { get; }
    }
}