using System;
using R3;
using ReactiveInputSystem;

namespace Script.TheLostSpirit.PlayerModule {
    public class GeneralActionsPresenter {
        readonly PlayerController         _playerController;
        readonly ActionMap.GeneralActions _general;

        public GeneralActionsPresenter(
            PlayerController         playerController,
            ActionMap.GeneralActions general,
            PlayerReference          lifetimeDependency
        ) {
            _playerController = playerController;
            _general          = general;

            var traversalActions = new[] {
                _general.FirstCircuit,
                _general.SecondCircuit
            };

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