using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class FormulaAddNodeUseCase : IUseCase<Void, FormulaAddNodeUseCase.Input>
    {
        readonly FormulaRepository _formulaRepository;

        public FormulaAddNodeUseCase(FormulaRepository formulaRepository) {
            _formulaRepository = formulaRepository;
        }

        public Void Execute(Input input) {
            var formula = _formulaRepository.GetByID(input.FormulaID);
            formula.AddNode(input.NodeID);

            return Void.Default;
        }


        public record struct Input(FormulaID FormulaID, NodeID NodeID) : IInput;
    }
}