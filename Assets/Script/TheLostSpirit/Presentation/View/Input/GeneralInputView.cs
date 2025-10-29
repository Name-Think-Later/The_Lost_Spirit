using System;
using R3;
using ReactiveInputSystem;
using TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Presentation.ViewModel.Player;

namespace TheLostSpirit.Presentation.View.Input
{
    public class GeneralInputView : IView<PlayerViewModel>, IDisposable
    {
        readonly ActionMap.GeneralActions _general;

        PlayerViewModel _inputViewModel;
        DisposableBag          _disposables;
        public FormulaInputView[] Formulas { get; }

        public GeneralInputView(ActionMap.GeneralActions general) {
            _general = general;

            Formulas = new[] {
                new FormulaInputView(_general.FirstFormula),
                new FormulaInputView(_general.SecondFormula)
            };
        }

        public void Bind(PlayerViewModel playerViewModel) {
            _inputViewModel = playerViewModel;

            var disposableBuilder = new DisposableBuilder();
            {
                MoveInputBinding().AddTo(ref disposableBuilder);

                DoJumpInputBinding().AddTo(ref disposableBuilder);
                ReleaseJumpInputBinding().AddTo(ref disposableBuilder);

                InteractInputBinding().AddTo(ref disposableBuilder);
            }

            disposableBuilder.Build().AddTo(ref _disposables);
        }

        public void Unbind() => _disposables.Dispose();

        IDisposable MoveInputBinding() {
            var moveAction = _general.Move;

            var press   = moveAction.PerformedAsObservable();
            var release = moveAction.CanceledAsObservable();

            var pressAndRelease =
                Observable.Merge(press, release);

            return pressAndRelease.Subscribe(context => {
                var value = context.ReadValue<float>();
                _inputViewModel.MoveInput(value);
            });
        }

        IDisposable DoJumpInputBinding() {
            var jumpAction = _general.Jump;

            var press = jumpAction.PerformedAsObservable();

            return press.Subscribe(_ => _inputViewModel.DoJumpInput());
        }

        IDisposable ReleaseJumpInputBinding() {
            var jumpAction = _general.Jump;

            var press   = jumpAction.PerformedAsObservable();
            var release = jumpAction.CanceledAsObservable();

            var unitRelease = release.AsUnitObservable();

            var holdingTime = TimeSpan.FromSeconds(0.5f);

            var holdingTimeoutOrRelease =
                press
                    .Select(_ => {
                            var holdingTimer = Observable.Timer(holdingTime);

                            return Observable.Amb(holdingTimer, unitRelease);
                        }
                    )
                    .Switch();

            return holdingTimeoutOrRelease.Subscribe(_ => _inputViewModel.ReleaseJumpInput());
        }

        IDisposable InteractInputBinding() {
            var interactAction = _general.Interact;

            var press = interactAction.PerformedAsObservable();

            return press.Subscribe(_ => _inputViewModel.InteractInput());
        }

        public void Dispose() => Unbind();
    }
}