using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Presentation.ViewModel.UseCasePort
{
    public interface IViewModelOnlyID<out TId> where TId : IIdentity
    {
        TId ID { get; }
    }
}
