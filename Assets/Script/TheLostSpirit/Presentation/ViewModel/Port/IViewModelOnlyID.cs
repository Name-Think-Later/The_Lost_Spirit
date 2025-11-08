using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Presentation.ViewModel.Port
{
    public interface IViewModelOnlyID<out TId> where TId : IIdentity
    {
        TId ID { get; }
    }
}
