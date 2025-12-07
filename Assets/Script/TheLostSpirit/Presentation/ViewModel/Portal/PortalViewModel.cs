using R3;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;
using UnityEngine;

namespace TheLostSpirit.Presentation.ViewModel.Portal
{
    public class PortalViewModel : IViewModel<PortalID>
    {
        readonly ReactiveProperty<bool>    _inFocus;
        readonly ReactiveProperty<Vector2> _debugLineDestination;

        public PortalID ID { get; }
        public ReadOnlyReactiveProperty<bool> InFocus => _inFocus;

        public IReadOnlyTransform DebugLineDestinationTarget { get; set; }


        public PortalViewModel(PortalID id) {
            ID       = id;
            _inFocus = new ReactiveProperty<bool>();
        }

        public void SetFocus(bool focus) {
            _inFocus.Value = focus;
        }
    }

    public static class ViewModelReferenceExtension
    {
        public static PortalViewModel AsViewModel(this IViewModelReference<PortalID> viewModelReference) {
            return (PortalViewModel)viewModelReference;
        }
    }
}