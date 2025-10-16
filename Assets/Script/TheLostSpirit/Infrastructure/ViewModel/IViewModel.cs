namespace TheLostSpirit.Infrastructure.ViewModel {
    public interface IViewModel<out T> where T : IIdentity {
        T ID { get; }
    }
}