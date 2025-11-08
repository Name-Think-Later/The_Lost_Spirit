using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Identify;

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
            var skillEntity = _skillFactory.Create(input.SkillIndex);
            _skillRepository.Save(skillEntity);

            return new Output(skillEntity.ID);
        }

        public record struct Input(int SkillIndex) : IInput;

        public record struct Output(SkillID SkillID) : IOutput;
    }
}