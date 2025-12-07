using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference
{
    public interface IViewModelReference<out TId> where TId : IRuntimeID
    {
        TId ID { get; }
    }
}