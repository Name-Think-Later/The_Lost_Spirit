using System;
using R3;
using ReactiveInputSystem;
using TheLostSpirit.ViewModel;

namespace TheLostSpirit {
    public class PlayerInputBinding : IDisposable {
        readonly ActionMap.GeneralActions _general;
        readonly PlayerViewModel          _playerViewModel;

        IDisposable _disposable;


        public PlayerInputBinding(
            ActionMap.GeneralActions general,
            PlayerViewModel          playerViewModel
        ) {
            _general         = general;
            _playerViewModel = playerViewModel;

            var disposableBuilder = new DisposableBuilder();
            {
                MoveInputBinding().AddTo(ref disposableBuilder);
                InteractInputBinding().AddTo(ref disposableBuilder);
            }
            _disposable = disposableBuilder.Build();
        }

        IDisposable MoveInputBinding() {
            var moveAction = _general.Move;
            var press      = moveAction.PerformedAsObservable();
            var release    = moveAction.CanceledAsObservable();

            var pressOrRelease =
                Observable.Merge(press, release);

            return pressOrRelease
                .Subscribe(_playerViewModel.AxisInput);
        }

        IDisposable InteractInputBinding() {
            var interactAction = _general.Interact;
            var press          = interactAction.PerformedAsObservable();

            return press.Subscribe(_playerViewModel.InteractInput);
        }

        public void Dispose() {
            _disposable.Dispose();
        }
    }
}