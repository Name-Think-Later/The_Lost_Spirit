using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class AddNodeInFormulaUseCase : IUseCase<Void, AddNodeInFormulaUseCase.Input>
    {
        readonly FormulaRepository _formulaRepository;
        readonly NodeRepository    _nodeRepository;

        public AddNodeInFormulaUseCase(
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

            if (nodeEntity.Skill is CoreID coreID)
                formulaEntity.AddCoreNode(nodeID, coreID);
            else
                formulaEntity.AddNode(nodeID);


            return Void.Default;
        }


        public record struct Input(FormulaID FormulaID, NodeID NodeID) : IInput;
    }
}