using R3;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Domain.Port.FormulaIOPolicy;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;
using UnityEngine;

namespace TheLostSpirit.Presentation.ViewModel.Formula
{
    public class FormulaViewModel : IViewModel<FormulaID>
    {
        readonly Subject<Unit> _cancel;
        readonly Subject<Unit> _perform;
        readonly Subject<Unit> _start;

        readonly TraverseFormulaUseCase _traverseFormulaUseCase;


        public FormulaViewModel(
            FormulaID              id,
            TraverseFormulaUseCase traverseFormulaUseCase
        ) {
            ID = id;

            _start   = new Subject<Unit>();
            _perform = new Subject<Unit>();
            _cancel  = new Subject<Unit>();

            _traverseFormulaUseCase = traverseFormulaUseCase;
        }

        public Observer<Unit> Start => _start.AsObserver();
        public Observer<Unit> Perform => _perform.AsObserver();
        public Observer<Unit> Cancel => _cancel.AsObserver();

        public FormulaID ID { get; }

        public FormulaViewModel WithIOPolicy(IFormulaIOPolicy formulaIOPolicy) {
            formulaIOPolicy
                .BindInput(_start, _perform, _cancel)
                .ToOutput(FormulaInput);

            return this;
        }

        public void FormulaInput() {
            var traverseNodeInput = new TraverseFormulaUseCase.Input(ID);
            _traverseFormulaUseCase.Execute(traverseNodeInput);
        }
    }

    public static partial class ViewModelReferenceExtension
    {
        public static FormulaViewModel AsViewModel(this IViewModelReference<FormulaID> viewModelReference) {
            return (FormulaViewModel)viewModelReference;
        }
    }
}