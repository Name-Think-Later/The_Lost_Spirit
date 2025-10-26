using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.ViewModel;

namespace TheLostSpirit.Presentation.View {
    public interface IView<in T>
        where T : IViewModel<IIdentity> {
        void Bind(T viewModel);

        void Unbind();
    }
}