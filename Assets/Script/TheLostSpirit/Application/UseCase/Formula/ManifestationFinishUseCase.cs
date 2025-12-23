using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class ManifestationFinishUseCase : IUseCase<Void, ManifestationFinishUseCase.Input>
    {
        readonly ManifestationRepository _manifestationRepository;

        public ManifestationFinishUseCase(ManifestationRepository manifestationRepository) {
            _manifestationRepository = manifestationRepository;
        }

        public Void Execute(Input input) {
            var manifestationEntity = _manifestationRepository.GetByID(input.ID);
            manifestationEntity.Finish();

            return Void.Default;
        }

        public record struct Input(ManifestationID ID) : IInput;
    }
}