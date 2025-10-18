using TheLostSpirit.Application.ObjectFactoryContract;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Context.ObjectFactory {
    public class PortalFactory : IPortalFactory {
        readonly PortalObjectContext  _origin;
        readonly PortalRepository     _portalRepository;
        readonly PortalViewModelStore _viewModelStore;

        public PortalFactory(
            PortalObjectContext  origin,
            PortalRepository     portalRepository,
            PortalViewModelStore viewModelStore
        ) {
            _origin           = origin;
            _portalRepository = portalRepository;
            _viewModelStore   = viewModelStore;
        }

        public PortalID CreateAndRegister() {
            var instance = Object.Instantiate(_origin);
            instance.Construct(_portalRepository, _viewModelStore);

            return instance.Produce();
        }
    }
}