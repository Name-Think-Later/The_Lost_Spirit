using System;
using R3;
using ReactiveInputSystem;
using Script.TheLostSpirit.CircuitSystem;

namespace Script.TheLostSpirit.PlayerModule {
    public class GeneralActionsEventHandler {
        readonly ActionMap.GeneralActions _general;
        readonly PlayerController         _playerController;

        public GeneralActionsEventHandler(
            ActionMap.GeneralActions general,
            PlayerController         playerController,
            PlayerReference          lifetimeDependency
        ) {
            _general          = general;
            _playerController = playerController;

            var disposableBuilder = Disposable.CreateBuilder();
            {
                ApplyMoveAction().AddTo(ref disposableBuilder);
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
    }
}