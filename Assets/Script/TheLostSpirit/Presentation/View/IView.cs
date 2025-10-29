using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.ViewModel;

namespace TheLostSpirit.Presentation.View
{
    public interface IView<in TViewModel> where TViewModel : IViewModel<IIdentity>
    {
        void Bind(TViewModel playerViewModel);

        void Unbind();
    }
}