using System;
using R3;
using Script.TheLostSpirit.Controller.PlayerController;
using Script.TheLostSpirit.InputModule;
using Script.TheLostSpirit.Reference.PlayerReference;

namespace Script.TheLostSpirit.Presenter.PlayerPresenter {
    public class PlayerManipulatePresenter {
        readonly PlayerController _playerController;

        public PlayerManipulatePresenter(
            ActionMap        actionMap,
            PlayerController playerController,
            PlayerReference  lifeTimeDependency
        ) {
            _playerController = playerController;

            var disposableBuilder = Disposable.CreateBuilder();
            {
                _ = new GeneralActionsBinding(playerController, actionMap.General).AddTo(ref disposableBuilder);
                PlayerMovementUpdateBinding().AddTo(ref disposableBuilder);
            }
            disposableBuilder.Build().AddTo(lifeTimeDependency);

            actionMap.Enable();
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