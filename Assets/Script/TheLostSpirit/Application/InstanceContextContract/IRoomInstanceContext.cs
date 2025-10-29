using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace Script.TheLostSpirit.Application.ObjectContextContract
{
    public interface IRoomInstanceContext
        : IInstanceContext<RoomID, RoomEntity, IViewModelOnlyID<RoomID>>
    {
        IPortalInstanceContext[] Portals { get; }
    }
}