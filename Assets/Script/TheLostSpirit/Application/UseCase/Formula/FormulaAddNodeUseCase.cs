using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class FormulaAddNodeUseCase : IUseCase<Void, FormulaAddNodeUseCase.Input>
    {
        readonly FormulaRepository _formulaRepository;
        readonly NodeRepository    _nodeRepository;

        public FormulaAddNodeUseCase(
            FormulaRepository formulaRepository,
            NodeRepository    nodeRepository
        ) {
            _formulaRepository = formulaRepository;
            _nodeRepository    = nodeRepository;
        }

        public Void Execute(Input input) {
            var nodeID        = input.NodeID;
            var formulaEntity = _formulaRepository.GetByID(input.FormulaID);
            var nodeEntity    = _nodeRepository.GetByID(nodeID);

            formulaEntity.AddNode(nodeID, nodeEntity.Skill);

            return Void.Default;
        }


        public record struct Input(FormulaID FormulaID, NodeID NodeID) : IInput;
    }
}