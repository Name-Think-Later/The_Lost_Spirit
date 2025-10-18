using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.ObjectFactoryContract {
    public interface IRoomFactory {
        void ResetCounter();
        RoomID CreateRandomAndRegister();
    }
}