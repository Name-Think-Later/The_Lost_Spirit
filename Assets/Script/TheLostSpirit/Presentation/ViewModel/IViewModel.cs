using Script.TheLostSpirit.Presentation.ViewModel.Port;
using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Presentation.ViewModel {
    public interface IViewModel<out T> : IViewModelOnlyID<T> where T : IIdentity { }
}