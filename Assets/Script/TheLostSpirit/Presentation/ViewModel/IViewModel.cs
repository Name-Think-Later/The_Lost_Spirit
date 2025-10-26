using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace TheLostSpirit.Presentation.ViewModel {
    public interface IViewModel<out T> : IViewModelOnlyID<T> where T : IIdentity { }
}