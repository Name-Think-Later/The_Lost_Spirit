namespace TheLostSpirit.Infrastructure.DomainDriven {
    public interface IEntity<out T> where T : IEntityID {
        T ID { get; }
    }
}