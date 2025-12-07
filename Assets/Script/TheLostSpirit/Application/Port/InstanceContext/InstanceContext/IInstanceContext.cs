using TheLostSpirit.Domain;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceContext
{
    public interface IInstanceContext<TId, out TEntity, out TViewModelReference>
        where TId : IRuntimeID
        where TEntity : IEntity<TId>
        where TViewModelReference : IViewModelReference<TId>
    {
        TEntity Entity { get; }
        TViewModelReference ViewModelReference { get; }
    }
}