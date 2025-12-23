using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class ManifestationDoFrameActionsUseCase : IUseCase<Void, ManifestationDoFrameActionsUseCase.Input>
    {
        readonly ManifestationRepository _manifestationRepository;

        public ManifestationDoFrameActionsUseCase(ManifestationRepository manifestationRepository) {
            _manifestationRepository = manifestationRepository;
        }

        public Void Execute(Input input) {
            var manifestationEntity = _manifestationRepository.GetByID(input.ManifestationID);
            manifestationEntity.DoFrameActions(input.Frame);

            return Void.Default;
        }

        public record struct Input(ManifestationID ManifestationID, int Frame) : IInput;
    }
}