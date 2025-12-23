using TheLostSpirit.Application.UseCase;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Presentation.ViewModel
{
    public class ManifestationViewModel : IViewModel<ManifestationID>
    {
        readonly ManifestationDoFrameActionsUseCase _manifestationDoFrameActionsUseCase;
        readonly ManifestationFinishUseCase         _manifestationFinishUseCase;
        public ManifestationID ID { get; }

        public ManifestationViewModel(
            ManifestationID                    id,
            ManifestationDoFrameActionsUseCase manifestationDoFrameActionsUseCase,
            ManifestationFinishUseCase         manifestationFinishUseCase
        ) {
            ID                                  = id;
            _manifestationDoFrameActionsUseCase = manifestationDoFrameActionsUseCase;
            _manifestationFinishUseCase         = manifestationFinishUseCase;
        }

        public void OnFrame(int frame) {
            var input = new ManifestationDoFrameActionsUseCase.Input(ID, frame);
            _manifestationDoFrameActionsUseCase.Execute(input);
        }

        public void OnFinish() {
            var input = new ManifestationFinishUseCase.Input(ID);
            _manifestationFinishUseCase.Execute(input);
        }
    }

    public static class ViewModelReferenceExtension
    {
        public static ManifestationViewModel AsViewModel(this IViewModelReference<ManifestationID> viewModelReference) {
            return (ManifestationViewModel)viewModelReference;
        }
    }
}