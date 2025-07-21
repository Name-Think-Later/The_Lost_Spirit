using System;
using Script.TheLostSpirit.Controller.PlayerController;
using UnityEngine.InputSystem;
using R3;
using ReactiveInputSystem;

namespace Script.TheLostSpirit.InputModule {
    public class GeneralActionsBinding : IDisposable {
        readonly PlayerController         _playerController;
        readonly ActionMap.GeneralActions _general;

        readonly IDisposable _disposable;

        public GeneralActionsBinding(
            PlayerController         playerController,
            ActionMap.GeneralActions general
        ) {
            _playerController = playerController;
            _general          = general;

            var disposableBuilder = Disposable.CreateBuilder();
            {
                MoveBinding().AddTo(ref disposableBuilder);
            }
            _disposable = disposableBuilder.Build();
        }

        IDisposable MoveBinding() {
            var movePerformed = _general.Move.PerformedAsObservable();
            var moveCanceled  = _general.Move.CanceledAsObservable();

            return Observable
                   .Merge(movePerformed, moveCanceled)
                   .Subscribe(context => {
                       var axis = context.ReadValue<float>();
                       _playerController.SetAxis(axis);
                   });
        }

        public void Dispose() {
            _disposable.Dispose();
        }
    }
}