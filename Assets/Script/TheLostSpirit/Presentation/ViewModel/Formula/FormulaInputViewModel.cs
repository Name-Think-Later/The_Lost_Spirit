using System;
using R3;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;
using UnityEngine;

namespace TheLostSpirit.Presentation.ViewModel.Formula
{
    public class FormulaInputViewModel : IViewModel<FormulaID>
    {
        readonly Subject<Unit> _start;
        readonly Subject<Unit> _perform;
        readonly Subject<Unit> _cancel;

        IDisposable    _disposable;
        IInputHandler  _inputHandler;
        IOutputHandler _outputHandler;

        public FormulaID ID { get; }
        public Observer<Unit> Start => _start.AsObserver();
        public Observer<Unit> Perform => _perform.AsObserver();
        public Observer<Unit> Cancel => _cancel.AsObserver();


        public FormulaInputViewModel(FormulaID id) {
            ID = id;

            _start   = new Subject<Unit>();
            _perform = new Subject<Unit>();
            _cancel  = new Subject<Unit>();
        }

        public FormulaInputViewModel WithInputHandler(IInputHandler inputHandler) {
            _inputHandler = inputHandler;

            _disposable?.Dispose();

            _disposable =
                _inputHandler
                    .ObservableInput(_start, _perform, _cancel)
                    .Subscribe(_ => _outputHandler.HandleOutput());

            return this;
        }

        public FormulaInputViewModel WithOutputHandler(IOutputHandler outputHandler) {
            _outputHandler = outputHandler;

            _outputHandler.OutputAction = FormulaInput;

            return this;
        }

        public void FormulaInput() {
            Debug.Log("Formula");
        }
    }

    public static class ViewModelOnlyIDExtension
    {
        public static FormulaInputViewModel TransformToViewModel(this IViewModelOnlyID<FormulaID> viewModelOnlyID) {
            return (FormulaInputViewModel)viewModelOnlyID;
        }
    }
}