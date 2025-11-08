using Script.TheLostSpirit.Presentation.ViewModel.Port;
using TheLostSpirit.Domain;
using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Application.Port.InstanceContext
{
    public interface IInstanceContext<TId, out TEntity, out TViewModelOnlyID>
        where TId : IIdentity
        where TEntity : IEntity<TId>
        where TViewModelOnlyID : IViewModelOnlyID<TId>
    {
        TEntity Entity { get; }
        TViewModelOnlyID ViewModelOnlyID { get; }
    }
}