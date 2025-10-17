using TheLostSpirit.Identify;

namespace TheLostSpirit.ViewModel {
    public interface IViewModel<out T> where T : IIdentity {
        T ID { get; }
    }
}