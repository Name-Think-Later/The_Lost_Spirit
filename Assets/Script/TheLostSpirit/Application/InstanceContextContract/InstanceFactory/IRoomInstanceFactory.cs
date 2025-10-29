using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Application.ObjectContextContract.ObjectFactory
{
    public interface IRoomInstanceFactory : IInstanceFactory<RoomID, IRoomInstanceContext>
    {
        void Reset();
    }
}