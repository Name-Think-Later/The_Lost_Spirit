using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.DomainDriven;

public interface IView<in T>
    where T : IViewModel<IEntityID>{
    public void Bind(T viewModel);
}