using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace Script.TheLostSpirit.Application.ObjectContextContract
{
    public interface IRoomObjectContext
        : IObjectContext<RoomID, RoomEntity, IViewModelOnlyID<RoomID>>
    {
        IPortalObjectContext[] Portals { get; }
    }
}