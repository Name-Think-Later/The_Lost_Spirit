using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Presentation.ViewModel.Formula
{
    public class AnchorViewModel : IViewModel<AnchorID>
    {
        public AnchorViewModel(AnchorID id) {
            ID = id;
        }

        public AnchorID ID { get; }
    }

    public static partial class ViewModelReferenceExtension
    {
        public static AnchorViewModel AsViewModel(this IViewModelReference<AnchorID> viewModelReference) {
            return (AnchorViewModel)viewModelReference;
        }
    }
}