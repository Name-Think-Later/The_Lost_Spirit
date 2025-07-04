using System;
using R3;
using Script.TheLostSpirit.Controller.PlayerController;
using Script.TheLostSpirit.InputModule;
using Script.TheLostSpirit.Reference.PlayerReference;

namespace Script.TheLostSpirit.Presenter.PlayerPresenter {
    public class PlayerPresenter {
        readonly ActionMap            _actionMap;
        readonly PlayerController     _playerController;
        readonly GeneralKeyMapActions _generalKeyMapActions;

        public PlayerPresenter(
            ActionMap        actionMap,
            PlayerController playerController,
            PlayerReference lifeTimeDependency
        ) {
            _actionMap            = actionMap;
            _playerController     = playerController;
            _generalKeyMapActions = new GeneralKeyMapActions(playerController);
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
                .GeneralKeymap
                .SetCallbacks(_generalKeyMapActions);
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