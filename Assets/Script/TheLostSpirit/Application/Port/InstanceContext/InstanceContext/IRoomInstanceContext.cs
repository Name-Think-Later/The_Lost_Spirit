using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceContext
{
    public interface IRoomInstanceContext
        : IInstanceContext<RoomID, RoomEntity, IViewModelReference<RoomID>>
    {
        IPortalInstanceContext[] Portals { get; }
    }
}