namespace TheLostSpirit.Infrastructure.Domain {
    public interface IEntity<out T> where T : IIdentity {
        T ID { get; }
    }
}