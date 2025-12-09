using System;
using R3;
using ReactiveInputSystem;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Formula;
using UnityEngine.InputSystem;

namespace TheLostSpirit.Presentation.View.Input
{
    public class FormulaInputView : IView<FormulaID, FormulaViewModel>, IDisposable
    {
        readonly InputAction _inputAction;

        IDisposable _disposable;

        public FormulaInputView(InputAction inputAction) {
            _inputAction = inputAction;
        }

        public void Dispose() {
            Unbind();
        }

        public void Bind(FormulaViewModel viewModel) {
            var start   = _inputAction.StartedAsObservable();
            var perform = _inputAction.PerformedAsObservable();
            var cancel  = _inputAction.CanceledAsObservable();

            var disposableBuilder = new DisposableBuilder();
            {
                start
                    .AsUnitObservable()
                    .Subscribe(viewModel.Start)
                    .AddTo(ref disposableBuilder);

                perform
                    .AsUnitObservable()
                    .Subscribe(viewModel.Perform)
                    .AddTo(ref disposableBuilder);

                cancel
                    .AsUnitObservable()
                    .Subscribe(viewModel.Cancel)
                    .AddTo(ref disposableBuilder);
            }
            _disposable = disposableBuilder.Build();
        }

        public void Unbind() {
            _disposable?.Dispose();
        }
    }
}