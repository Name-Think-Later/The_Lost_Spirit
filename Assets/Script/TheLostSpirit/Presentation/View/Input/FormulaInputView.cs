using System;
using R3;
using ReactiveInputSystem;
using Script.TheLostSpirit.Presentation.ViewModel.Formula;
using TheLostSpirit.Identify;
using UnityEngine.InputSystem;

namespace Script.TheLostSpirit.Presentation.View.Input {
    public class FormulaInputView : IView<FormulaID, FormulaViewModel>, IDisposable {
        readonly InputAction _inputAction;

        IDisposable _disposable;

        public FormulaInputView(InputAction inputAction) {
            _inputAction = inputAction;
        }

        public void Bind(FormulaViewModel playerViewModel) {
            var start   = _inputAction.StartedAsObservable();
            var perform = _inputAction.PerformedAsObservable();
            var cancel  = _inputAction.CanceledAsObservable();

            var disposableBuilder = new DisposableBuilder();
            {
                start
                    .AsUnitObservable()
                    .Subscribe(playerViewModel.Start)
                    .AddTo(ref disposableBuilder);

                perform
                    .AsUnitObservable()
                    .Subscribe(playerViewModel.Perform)
                    .AddTo(ref disposableBuilder);

                cancel
                    .AsUnitObservable()
                    .Subscribe(playerViewModel.Cancel)
                    .AddTo(ref disposableBuilder);
            }
            _disposable = disposableBuilder.Build();
        }

        public void Unbind() => _disposable?.Dispose();
        public void Dispose() => Unbind();
    }
}