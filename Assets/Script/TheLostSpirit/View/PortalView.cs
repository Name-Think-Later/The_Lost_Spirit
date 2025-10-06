using R3;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.ViewModel.Portal;
using UnityEngine;

namespace TheLostSpirit.View {
    public class PortalView : MonoBehaviour, IView<PortalViewModel> {
        [SerializeField]
        SpriteRenderer _spriteRenderer;

        public void Bind(PortalViewModel viewModel) {
            var inFocus = viewModel.InFocus;

            inFocus
                .Where(b => b)
                .Subscribe(_ => _spriteRenderer.color = Color.green);

            inFocus
                .Where(b => !b)
                .Subscribe(_ => _spriteRenderer.color = Color.white);
        }
    }
}