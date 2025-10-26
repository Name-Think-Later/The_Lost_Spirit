using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace TheLostSpirit.Presentation.ViewModel.Room {
    public class RoomViewModel : IViewModel<RoomID> {
        public RoomViewModel(RoomID id) {
            ID = id;
        }

        public RoomID ID { get; }

    }

    public static class ViewModelOnlyIDExtension {
        public static RoomViewModel TransformToViewModel(this IViewModelOnlyID<RoomID> viewModelOnlyID) {
            return (RoomViewModel)viewModelOnlyID;
        }
    }
}