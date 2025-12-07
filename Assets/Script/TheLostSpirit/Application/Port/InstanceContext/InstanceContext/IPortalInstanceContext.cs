using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceContext
{
    public interface IPortalInstanceContext
        : IInstanceContext<PortalID, PortalEntity, IViewModelReference<PortalID>>
    { }
}