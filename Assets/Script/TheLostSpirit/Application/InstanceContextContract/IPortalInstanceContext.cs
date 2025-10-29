using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace Script.TheLostSpirit.Application.ObjectContextContract
{
    public interface IPortalInstanceContext
        : IInstanceContext<PortalID, PortalEntity, IViewModelOnlyID<PortalID>>
    { }
}