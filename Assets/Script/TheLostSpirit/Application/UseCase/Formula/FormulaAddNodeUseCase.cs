using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class FormulaAddNodeUseCase : IUseCase<Void, FormulaAddNodeUseCase.Input>
    {
        readonly FormulaRepository _formulaRepository;
        readonly NodeRepository    _nodeRepository;

        public FormulaAddNodeUseCase(FormulaRepository formulaRepository) {
            _formulaRepository = formulaRepository;
        }

        public Void Execute(Input input) {
            var nodeID        = input.NodeID;
            var formulaEntity = _formulaRepository.GetByID(input.FormulaID);
            formulaEntity.AddNode(nodeID);

            var nodeEntity = _nodeRepository.GetByID(nodeID);
            if (nodeEntity.Skill is CoreID coreID)
                formulaEntity.CoreNode = (nodeID, coreID);


            return Void.Default;
        }


        public record struct Input(FormulaID FormulaID, NodeID NodeID) : IInput;
    }
}