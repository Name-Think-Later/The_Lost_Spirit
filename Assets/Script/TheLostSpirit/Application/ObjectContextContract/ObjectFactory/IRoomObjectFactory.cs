using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Application.ObjectContextContract.ObjectFactory
{
    public interface IRoomObjectFactory : IObjectFactory<RoomID, IRoomObjectContext>
    {
        void Reset();
    }
}