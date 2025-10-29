using System;
using R3;
using TheLostSpirit.Presentation.ViewModel.Portal;
using UnityEngine;

namespace TheLostSpirit.Presentation.View {
    public class PortalView : MonoBehaviour, IView<PortalViewModel> {
        [SerializeField]
        SpriteRenderer _spriteRenderer;

        IDisposable _disposable;

        public void Bind(PortalViewModel playerViewModel) {
            var inFocus = playerViewModel.InFocus;

            var disposableBuilder = new DisposableBuilder();
            {
                inFocus
                    .Where(b => b)
                    .Subscribe(_ => _spriteRenderer.color = Color.green)
                    .AddTo(ref disposableBuilder);

                inFocus
                    .Where(b => !b)
                    .Subscribe(_ => _spriteRenderer.color = Color.white)
                    .AddTo(ref disposableBuilder);

                Observable
                    .EveryUpdate()
                    .Subscribe(_ => {
                        if (playerViewModel.DebugLineDestinationTarget == null) return;
                        Debug.DrawLine(transform.position, playerViewModel.DebugLineDestinationTarget.Position);
                    })
                    .AddTo(ref disposableBuilder);
            }

            _disposable = disposableBuilder.Build();
            _disposable.AddTo(this);
        }

        public void Unbind() => _disposable.Dispose();
    }
}