using TheLostSpirit.Infrastructure.ViewModel;

namespace TheLostSpirit.Infrastructure {
    public interface IView<in T>
        where T : IViewModel<IIdentity>{
        public void Bind(T viewModel);
    }

}