using System;
using R3;
using Script.TheLostSpirit.Domain.ViewModelPort;
using Script.TheLostSpirit.Presentation.ViewModel.UseCasePort;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Identify;
using UnityEngine;

namespace Script.TheLostSpirit.Presentation.ViewModel.Formula
{
    public class FormulaViewModel : IViewModel<FormulaID>
    {
        readonly Subject<Unit> _start;
        readonly Subject<Unit> _perform;
        readonly Subject<Unit> _cancel;

        readonly TraverseNodeUseCase _traverseNodeUseCase;

        public FormulaID ID { get; }
        public Observer<Unit> Start => _start.AsObserver();
        public Observer<Unit> Perform => _perform.AsObserver();
        public Observer<Unit> Cancel => _cancel.AsObserver();


        public FormulaViewModel(
            FormulaID           id,
            TraverseNodeUseCase traverseNodeUseCase
        ) {
            ID = id;

            _start   = new Subject<Unit>();
            _perform = new Subject<Unit>();
            _cancel  = new Subject<Unit>();

            _traverseNodeUseCase = traverseNodeUseCase;
        }

        public FormulaViewModel WithIOPolicy(IFormulaIOPolicy formulaIOPolicy) {
            formulaIOPolicy
                .BindInput(_start, _perform, _cancel)
                .ToOutput(FormulaInput);

            return this;
        }

        public void FormulaInput() {
            Debug.Log("Formula");
            var traverseNodeInput = new TraverseNodeUseCase.Input(ID);
            _traverseNodeUseCase.Execute(traverseNodeInput);
        }
    }

    public static class ViewModelOnlyIDExtension
    {
        public static FormulaViewModel TransformToViewModel(this IViewModelOnlyID<FormulaID> viewModelOnlyID) {
            return (FormulaViewModel)viewModelOnlyID;
        }
    }
}