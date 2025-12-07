using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Domain;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceFactory
{
    public interface IInstanceFactory<TId, out TObjectContext>
        where TId : IRuntimeID
        where TObjectContext : IInstanceContext<TId, IEntity<TId>, IViewModelReference<TId>>
    {
        TObjectContext Create();
    }
}