using System;
using R3;
using ReactiveInputSystem;

namespace Script.TheLostSpirit.PlayerModule {
    public class PlayerControlInputEventHandler {
        readonly ActionMap.GeneralActions _general;
        readonly PlayerController         _playerController;

        public PlayerControlInputEventHandler(
            ActionMap.GeneralActions general,
            PlayerController         playerController,
            PlayerReference          lifetimeDependency
        ) {
            _general          = general;
            _playerController = playerController;

            var disposableBuilder = Disposable.CreateBuilder();
            {
                ApplyMoveAction().AddTo(ref disposableBuilder);
                ApplyMovementUpdate().AddTo(ref disposableBuilder);
            }
            disposableBuilder.Build().AddTo(lifetimeDependency);
        }

        IDisposable ApplyMoveAction() {
            var moveAction    = _general.Move;
            var movePerformed = moveAction.PerformedAsObservable();
            var moveCanceled  = moveAction.CanceledAsObservable();

            return Observable
                   .Merge(movePerformed, moveCanceled)
                   .Subscribe(context => {
                       var axis = context.ReadValue<float>();
                       _playerController.SetAxis(axis);
                   });
        }

        private IDisposable ApplyMovementUpdate() {
            return Observable
                   .EveryUpdate(UnityFrameProvider.FixedUpdate)
                   .Subscribe(_ => {
                       _playerController.ApplyVelocity();
                   });
        }
    }
}