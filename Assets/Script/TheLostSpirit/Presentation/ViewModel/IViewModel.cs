using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Presentation.ViewModel
{
    public interface IViewModel<out T> : IViewModelReference<T> where T : IRuntimeID
    { }
}