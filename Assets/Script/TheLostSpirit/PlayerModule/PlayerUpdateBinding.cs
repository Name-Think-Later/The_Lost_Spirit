using System;
using R3;

namespace Script.TheLostSpirit.PlayerModule {
    public class PlayerUpdateBinding : IDisposable {
        readonly IDisposable _disposable;

        public PlayerUpdateBinding(
            PlayerController playerController
        ) {
            _disposable =
                Observable
                    .EveryUpdate(UnityFrameProvider.FixedUpdate)
                    .Subscribe(_ => {
                        playerController.UpdateVelocity();
                    });
        }

        public void Dispose() {
            _disposable.Dispose();
        }
    }
}