using R3;
using TheLostSpirit.IDentify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.DomainDriven;
using TheLostSpirit.ViewModel;
using UnityEngine;

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