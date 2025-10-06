using R3;
using TheLostSpirit.IDentify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.DomainDriven;

namespace TheLostSpirit.ViewModel {
    public class PortalViewModel : IViewModel<PortalID>{
        readonly ReactiveProperty<bool> _inFocus;

        public PortalID ID { get; }

        public PortalViewModel(PortalID id) {
            ID       = id;
            _inFocus = new ReactiveProperty<bool>();
        }

        public void SetFocus(bool focus) {
            _inFocus.Value = focus;
        }

        public ReadOnlyReactiveProperty<bool> InFocus => _inFocus;
    }
}