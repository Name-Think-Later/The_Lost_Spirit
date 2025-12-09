using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Presentation.ViewModel
{
    public class ManifestationViewModel : IViewModel<ManifestationID>
    {
        readonly ManifestationTriggerEffectUseCase _manifestationTriggerEffectUseCase;

        public ManifestationViewModel(ManifestationID id) {
            ID = id;
        }

        public void TriggerEffect(int effectIndex) {
            var input = new ManifestationTriggerEffectUseCase.Input(ID, effectIndex);
            _manifestationTriggerEffectUseCase.Execute(input);
        }

        public ManifestationID ID { get; }
    }

    public static class ViewModelReferenceExtension
    {
        public static ManifestationViewModel AsViewModel(this IViewModelReference<ManifestationID> viewModelReference) {
            return (ManifestationViewModel)viewModelReference;
        }
    }
}