using Script.TheLostSpirit.Application.Port.InstanceContext;
using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Application.Port.InstanceFactory
{
    public interface IRoomInstanceFactory : IInstanceFactory<RoomID, IRoomInstanceContext>
    {
        void Reset();
    }
}