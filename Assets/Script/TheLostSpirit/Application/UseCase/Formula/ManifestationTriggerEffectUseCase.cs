using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class ManifestationTriggerEffectUseCase : IUseCase<Void, ManifestationTriggerEffectUseCase.Input>
    {
        ManifestationRepository _manifestationRepository;

        public ManifestationTriggerEffectUseCase(ManifestationRepository manifestationRepository) {
            _manifestationRepository = manifestationRepository;
        }

        public Void Execute(Input input) {
            return Void.Default;
        }

        public record struct Input(ManifestationID ManifestationID, int EffectIndex) : IInput;
    }
}