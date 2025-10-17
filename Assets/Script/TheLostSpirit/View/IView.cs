using TheLostSpirit.Identify;
using TheLostSpirit.ViewModel;

namespace TheLostSpirit.View {
    public interface IView<in T>
        where T : IViewModel<IIdentity>{
        public void Bind(T viewModel);
    }

}