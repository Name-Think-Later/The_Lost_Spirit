using TheLostSpirit.Infrastructure.DomainDriven;

namespace TheLostSpirit.Infrastructure {
    public interface IViewModel<T> where T : IEntityID {
        T ID { get; }
    }
}