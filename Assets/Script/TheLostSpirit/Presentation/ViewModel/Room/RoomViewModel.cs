using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Presentation.ViewModel.Room
{
    public class RoomViewModel : IViewModel<RoomID>
    {
        public RoomViewModel(RoomID id) {
            ID = id;
        }

        public RoomID ID { get; }
    }

    public static class ViewModelReferenceExtension
    {
        public static RoomViewModel AsViewModel(this IViewModelReference<RoomID> viewModelReference) {
            return (RoomViewModel)viewModelReference;
        }
    }
}