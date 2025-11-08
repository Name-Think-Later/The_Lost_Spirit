using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class NodeContainSkillUseCase : IUseCase<Void, NodeContainSkillUseCase.Input>
    {
        readonly NodeRepository _nodeRepository;

        public NodeContainSkillUseCase(NodeRepository nodeRepository) {
            _nodeRepository = nodeRepository;
        }

        public Void Execute(Input input) {
            var nodeEntity = _nodeRepository.GetByID(input.NodeID);
            nodeEntity.Skill = input.SkillID;

            return Void.Default;
        }

        public record struct Input(NodeID NodeID, SkillID SkillID) : IInput;
    }
}