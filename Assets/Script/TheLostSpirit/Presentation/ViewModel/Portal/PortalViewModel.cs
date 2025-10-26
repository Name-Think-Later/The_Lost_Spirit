using R3;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Presentation.IDOnlyViewModel;
using UnityEngine;

namespace TheLostSpirit.Presentation.ViewModel.Portal {
    public class PortalViewModel : IViewModel<PortalID> {
        readonly ReactiveProperty<bool>    _inFocus;
        readonly ReactiveProperty<Vector2> _debugLineDestination;

        public PortalID ID { get; }
        public ReadOnlyReactiveProperty<bool> InFocus => _inFocus;

        public ReadOnlyTransform DebugLineDestinationTarget { get; set; }


        public PortalViewModel(PortalID id) {
            ID       = id;
            _inFocus = new ReactiveProperty<bool>();
        }

        public void SetFocus(bool focus) {
            _inFocus.Value = focus;
        }
    }

    public static class ViewModelOnlyIDExtension {
        public static PortalViewModel TransformToViewModel(this IViewModelOnlyID<PortalID> viewModelOnlyID) {
            return (PortalViewModel)viewModelOnlyID;
        }
    }
}