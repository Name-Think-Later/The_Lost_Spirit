using Script.TheLostSpirit.Application.Port.InstanceContext;
using Script.TheLostSpirit.Presentation.ViewModel.UseCasePort;
using TheLostSpirit.Domain;
using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Application.Port.InstanceFactory
{
    public interface IInstanceFactory<TId, out TObjectContext>
        where TId : IIdentity
        where TObjectContext : IInstanceContext<TId, IEntity<TId>, IViewModelOnlyID<TId>>
    {
        TObjectContext Create();
    }
}