using R3;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.ViewModel;
using UnityEngine;

namespace TheLostSpirit.ViewModel.Portal {
    public class PortalViewModel : IViewModel<PortalID> {
        readonly ReactiveProperty<bool>    _inFocus;
        readonly ReactiveProperty<Vector2> _debugLineDestination;

        public PortalID ID { get; }
        public ReadOnlyReactiveProperty<bool> InFocus => _inFocus;

        public Vector2 DebugLineDestination { get; set; }


        public PortalViewModel(PortalID id) {
            ID       = id;
            _inFocus = new ReactiveProperty<bool>();
        }

        public void SetFocus(bool focus) {
            _inFocus.Value = focus;
        }
    }
}