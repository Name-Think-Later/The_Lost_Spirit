using System;
using R3;
using ReactiveInputSystem;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.ViewModel;
using UnityEngine;

namespace TheLostSpirit {
    public class GeneralInputView : IDisposable, IView<PlayerViewModel> {
        readonly ActionMap.GeneralActions _general;

        PlayerViewModel _viewModel;
        IDisposable     _disposable;

        public GeneralInputView(ActionMap.GeneralActions general) {
            _general = general;
        }

        public void Bind(PlayerViewModel viewModel) {
            _viewModel = viewModel;

            var disposableBuilder = new DisposableBuilder();
            {
                MoveInputBinding().AddTo(ref disposableBuilder);

                DoJumpInputBinding().AddTo(ref disposableBuilder);
                ReleaseJumpInputBinding().AddTo(ref disposableBuilder);

                InteractInputBinding().AddTo(ref disposableBuilder);
            }
            _disposable = disposableBuilder.Build();
        }


        IDisposable MoveInputBinding() {
            var moveAction = _general.Move;

            var press   = moveAction.PerformedAsObservable();
            var release = moveAction.CanceledAsObservable();

            var pressAndRelease =
                Observable.Merge(press, release);

            return pressAndRelease.Subscribe(context => {
                var value = context.ReadValue<float>();
                _viewModel.MoveInput(value);
            });
        }

        IDisposable DoJumpInputBinding() {
            var jumpAction = _general.Jump;

            var press = jumpAction.PerformedAsObservable();

            return press.Subscribe(_ => _viewModel.DoJumpInput());
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

            return holdingTimeoutOrRelease.Subscribe(_ => _viewModel.ReleaseJumpInput());
        }

        IDisposable InteractInputBinding() {
            var interactAction = _general.Interact;

            var press = interactAction.PerformedAsObservable();

            return press.Subscribe(_ => _viewModel.InteractInput());
        }

        public void Dispose() => _disposable.Dispose();
    }
}