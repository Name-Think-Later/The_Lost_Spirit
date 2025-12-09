using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel;

namespace TheLostSpirit.Presentation.View
{
    public interface IView<TId, in TViewModel> where TViewModel : IViewModel<TId> where TId : IRuntimeID
    {
        void Bind(TViewModel viewModel);

        void Unbind();
    }
}