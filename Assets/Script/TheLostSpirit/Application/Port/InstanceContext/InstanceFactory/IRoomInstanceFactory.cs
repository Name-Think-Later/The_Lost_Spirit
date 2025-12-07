using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceFactory
{
    public interface IRoomInstanceFactory : IInstanceFactory<RoomID, IRoomInstanceContext>
    {
        void Reset();
    }
}