using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Presentation.ViewModel
{
    public class ManifestationViewModel : IViewModel<ManifestationID>
    {
        public ManifestationID ID { get; }

        public ManifestationViewModel(ManifestationID id) {
            ID = id;
        }

    }

    public static class ViewModelReferenceExtension
    {
        public static ManifestationViewModel AsViewModel(this IViewModelReference<ManifestationID> viewModelReference) {
            return (ManifestationViewModel)viewModelReference;
        }
    }
}