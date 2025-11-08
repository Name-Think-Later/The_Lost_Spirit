using Script.TheLostSpirit.Presentation.ViewModel.UseCasePort;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Application.Port.InstanceContext
{
    public interface IRoomInstanceContext
        : IInstanceContext<RoomID, RoomEntity, IViewModelOnlyID<RoomID>>
    {
        IPortalInstanceContext[] Portals { get; }
    }
}