using Script.TheLostSpirit.Presentation.ViewModel.UseCasePort;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Application.Port.InstanceContext
{
    public interface IPortalInstanceContext
        : IInstanceContext<PortalID, PortalEntity, IViewModelOnlyID<PortalID>>
    { }
}