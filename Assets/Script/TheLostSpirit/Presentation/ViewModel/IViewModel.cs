using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Presentation.ViewModel
{
    public interface IViewModel<out T> : IViewModelReference<T> where T : IRuntimeID
    { }
}