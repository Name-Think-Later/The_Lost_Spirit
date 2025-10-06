using TheLostSpirit.Infrastructure.DomainDriven;

namespace TheLostSpirit.Infrastructure {
    public interface IView<in T>
        where T : IViewModel<IEntityID>{
        public void Bind(T viewModel);
    }
}