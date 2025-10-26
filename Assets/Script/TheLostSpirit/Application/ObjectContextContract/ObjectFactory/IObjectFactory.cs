using TheLostSpirit.Domain;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace Script.TheLostSpirit.Application.ObjectContextContract.ObjectFactory
{
    public interface IObjectFactory<TId, out TObjectContext>
        where TId : IIdentity
        where TObjectContext : IObjectContext<TId, IEntity<TId>, IViewModelOnlyID<TId>>
    {
        TObjectContext Create();
    }
}