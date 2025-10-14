using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.ViewModel;

namespace TheLostSpirit.ViewModel.Room {
    public class RoomViewModel : IViewModel<RoomID> {
        public RoomViewModel(RoomID id) {
            ID = id;
        }

        public RoomID ID { get; }
    }
}