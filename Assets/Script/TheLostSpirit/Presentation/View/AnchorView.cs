using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Formula;
using UnityEngine;

namespace TheLostSpirit.Presentation.View
{
    public class AnchorView : MonoBehaviour, IView<AnchorID, AnchorViewModel>
    {
        public void Bind(AnchorViewModel viewModel) { }
        public void Unbind() { }
    }
}