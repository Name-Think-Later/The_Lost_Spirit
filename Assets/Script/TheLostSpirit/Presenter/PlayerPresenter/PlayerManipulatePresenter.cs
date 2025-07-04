using System;
using R3;
using Script.TheLostSpirit.Controller.PlayerController;
using Script.TheLostSpirit.InputModule;
using Script.TheLostSpirit.Reference.PlayerReference;

namespace Script.TheLostSpirit.Presenter.PlayerPresenter {
    public class PlayerManipulatePresenter {
        readonly ActionMap            _actionMap;
        readonly PlayerController     _playerController;
        readonly GeneralActions _generalActions;

        public PlayerManipulatePresenter(
            ActionMap        actionMap,
            PlayerController playerController,
            PlayerReference lifeTimeDependency
        ) {
            _actionMap            = actionMap;
            _playerController     = playerController;
            _generalActions = new GeneralActions(playerController);
            
            ActionBinding();
            _actionMap.Enable();

            var disposableBuilder = Disposable.CreateBuilder();
            {
                PlayerMovementUpdateBinding().AddTo(ref disposableBuilder);
            }
            disposableBuilder.Build().AddTo(lifeTimeDependency);
        }

        private void ActionBinding() {
            _actionMap
                .General
                .SetCallbacks(_generalActions);
        }

        private IDisposable PlayerMovementUpdateBinding() {
            return Observable
                   .EveryUpdate(UnityFrameProvider.FixedUpdate)
                   .Subscribe(_ => {
                       _playerController.ApplyVelocity();
                   });
            
        }
    }
}