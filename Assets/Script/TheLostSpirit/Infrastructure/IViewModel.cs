using TheLostSpirit.Infrastructure.DomainDriven;

namespace TheLostSpirit.Infrastructure {
    public interface IViewModel<out T> where T : IEntityID {
        T ID { get; }
    }
}