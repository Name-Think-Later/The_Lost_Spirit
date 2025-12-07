using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel;
using UnityEngine;

namespace TheLostSpirit.Presentation.View
{
    public class ManifestationView : MonoBehaviour, IView<ManifestationID, ManifestationViewModel>
    {
        public void Bind(ManifestationViewModel playerViewModel) { }
        public void Unbind() { }
    }
}