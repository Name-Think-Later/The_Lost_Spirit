using TheLostSpirit.Domain;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace Script.TheLostSpirit.Application.ObjectContextContract
{
    public interface IObjectContext<TId, out TEntity, out TViewModelOnlyID>
        where TId : IIdentity
        where TEntity : IEntity<TId>
        where TViewModelOnlyID : IViewModelOnlyID<TId>
    {
        TEntity Entity { get; }
        TViewModelOnlyID ViewModelOnlyID { get; }
    }
}