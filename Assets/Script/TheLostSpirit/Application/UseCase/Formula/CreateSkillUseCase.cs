using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class CreateSkillUseCase : IUseCase<CreateSkillUseCase.Output, CreateSkillUseCase.Input>
    {
        readonly SkillFactory    _skillFactory;
        readonly SkillRepository _skillRepository;

        public CreateSkillUseCase(
            SkillFactory    skillFactory,
            SkillRepository skillRepository
        ) {
            _skillFactory    = skillFactory;
            _skillRepository = skillRepository;
        }

        public Output Execute(Input input) {
            var skillEntity = _skillFactory.Create(input.Id);
            _skillRepository.Save(skillEntity);

            return new Output(skillEntity.ID);
        }

        public record struct Input(SkillSpecificationID Id) : IInput;

        public record struct Output(ISkillID SkillID) : IOutput;
    }
}