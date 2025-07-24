using System;
using R3;

namespace Script.TheLostSpirit.PlayerModule {
    public class PlayerPhysicPresenter {
        readonly PlayerController _playerController;

        public PlayerPhysicPresenter(
            PlayerController playerController,
            PlayerReference  lifeTimeDependency
        ) {
            _playerController = playerController;

            var disposableBuilder = Disposable.CreateBuilder();
            {
                
                ApplyMovementUpdate().AddTo(ref disposableBuilder);
            }
            disposableBuilder.Build().AddTo(lifeTimeDependency);
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