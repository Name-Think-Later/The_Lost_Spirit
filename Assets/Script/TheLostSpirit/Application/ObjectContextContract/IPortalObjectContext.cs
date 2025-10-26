using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace Script.TheLostSpirit.Application.ObjectContextContract
{
    public interface IPortalObjectContext
        : IObjectContext<PortalID, PortalEntity, IViewModelOnlyID<PortalID>>
    { }
}