using Script.TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Presentation.View
{
    public interface IView<TId, in TViewModel> where TViewModel : IViewModel<TId> where TId : IIdentity
    {
        void Bind(TViewModel playerViewModel);

        void Unbind();
    }
}