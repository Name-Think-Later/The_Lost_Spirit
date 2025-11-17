using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using MoreLinq;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class TraverseFormulaUseCase : IUseCase<Void, TraverseFormulaUseCase.Input>
    {
        readonly FormulaRepository _formulaRepository;

        public TraverseFormulaUseCase(
            FormulaRepository formulaRepository
        ) {
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