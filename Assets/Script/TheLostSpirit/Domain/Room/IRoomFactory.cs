using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Room {
    public interface IRoomFactory {
        RoomID CreateRandom();
    }
}