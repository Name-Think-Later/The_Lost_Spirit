using System;
using R3;
using ReactiveInputSystem;
using Script.TheLostSpirit.EventBusModule;
using UnityEngine.InputSystem;

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

                for (int i = 0; i < traversalActions.Length; i++) {
                    ApplyTraversalAction(traversalActions[i], i).AddTo(ref disposableBuilder);
                }
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

        IDisposable ApplyTraversalAction(InputAction traversalAction, int index) {
            var traversalPerformed = traversalAction.PerformedAsObservable();
            var traversalCancel    = traversalAction.CanceledAsObservable();

            var performedEvent =
                traversalPerformed.Do(_ => EventBus.Publish<TraversalInputEvent.PerformedEvent>());

            var cancelEvent =
                traversalCancel.Do(_ => EventBus.Publish<TraversalInputEvent.CancelEvent>());

            return Observable
                   .Merge(performedEvent, cancelEvent)
                   .Subscribe();
        }
    }
}