using TheLostSpirit.Domain;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace Script.TheLostSpirit.Application.ObjectContextContract.ObjectFactory
{
    public interface IInstanceFactory<TId, out TObjectContext>
        where TId : IIdentity
        where TObjectContext : IInstanceContext<TId, IEntity<TId>, IViewModelOnlyID<TId>>
    {
        TObjectContext Create();
    }
}