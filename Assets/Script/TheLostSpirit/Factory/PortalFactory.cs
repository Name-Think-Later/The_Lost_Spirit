using TheLostSpirit.Context.Portal;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identify;
using TheLostSpirit.ViewModel.Portal;
using UnityEngine;

namespace TheLostSpirit.Factory {
    public class PortalFactory : IPortalFactory {
        readonly PortalObjectContext     _origin;
        readonly PortalRepository     _portalRepository;
        readonly PortalViewModelStore _viewModelStore;

        public PortalFactory(
            PortalObjectContext     origin,
            PortalRepository     portalRepository,
            PortalViewModelStore viewModelStore
        ) {
            _origin           = origin;
            _portalRepository = portalRepository;
            _viewModelStore   = viewModelStore;
        }

        public PortalID Create() {
            var instance = Object.Instantiate(_origin);
            instance.Construct(_portalRepository, _viewModelStore);

            return instance.Produce();
        }
    }
}