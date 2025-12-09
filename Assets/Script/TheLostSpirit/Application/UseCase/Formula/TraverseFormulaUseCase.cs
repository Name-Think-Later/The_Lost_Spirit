using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class TraverseFormulaUseCase : IUseCase<Void, TraverseFormulaUseCase.Input>
    {
        readonly FormulaRepository _formulaRepository;

        public TraverseFormulaUseCase(FormulaRepository formulaRepository) {
            _formulaRepository = formulaRepository;
        }

        public Void Execute(Input input) {
            var formulaEntity = _formulaRepository.GetByID(input.FormulaID);
            formulaEntity.Traverse();

            return Void.Default;
        }


        public record struct Input(FormulaID FormulaID) : IInput;
    }
}