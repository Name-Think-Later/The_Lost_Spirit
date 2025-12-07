using Cysharp.Threading.Tasks;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class ActiveSkillUseCase : IUseCaseAsync<Void, ActiveSkillUseCase.Input>
    {
        readonly SkillRepository _skillRepository;

        public ActiveSkillUseCase(SkillRepository skillRepository) {
            _skillRepository = skillRepository;
        }

        public async UniTask<Void> Execute(Input input) {
            var skillEntity = _skillRepository.GetByID(input.SkillID);
            await skillEntity.Activate(input.Payload);

            return Void.Default;
        }

        public record struct Input(ISkillID SkillID, FormulaPayload Payload) : IInput;
    }
}