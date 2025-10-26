using TheLostSpirit.Identify;

namespace TheLostSpirit.Presentation.IDOnlyViewModel {
    public interface IViewModelOnlyID<out TId> where TId : IIdentity {
        TId ID { get; }
    }
}