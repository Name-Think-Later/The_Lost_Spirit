using Script.TheLostSpirit.Presentation.ViewModel.UseCasePort;
using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Presentation.ViewModel {
    public interface IViewModel<out T> : IViewModelOnlyID<T> where T : IIdentity { }
}