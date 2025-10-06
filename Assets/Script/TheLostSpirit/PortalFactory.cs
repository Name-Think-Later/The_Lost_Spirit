using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.ViewModel.Portal;

namespace TheLostSpirit {
    public class PortalFactory {
        readonly PortalRepository     _portalRepository;
        readonly PortalViewModelStore _portalViewModelStore;

        public PortalFactory(
            PortalRepository     portalRepository,
            PortalViewModelStore portalViewModelStore
        ) {
            _portalRepository     = portalRepository;
            _portalViewModelStore = portalViewModelStore;
        }

        public PortalID Create(PortalMono portalMono, IView<PortalViewModel> view) {
            var id = new PortalID();

            var entity = new PortalEntity(id, portalMono);
            _portalRepository.Add(entity);

            var viewModel = new PortalViewModel(id);
            _portalViewModelStore.Add(viewModel);

            view.Bind(viewModel);

            return id;
        }
    }
}