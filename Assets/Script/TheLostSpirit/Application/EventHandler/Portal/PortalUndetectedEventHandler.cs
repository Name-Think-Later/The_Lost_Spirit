using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Presentation.ViewModel.Portal;

namespace TheLostSpirit.Application.EventHandler.Portal
{
    public class PortalUndetectedEventHandler : DomainEventHandler<PortalUndetectedEvent>
    {
        readonly PortalViewModelStore _portalViewModelStore;

        public PortalUndetectedEventHandler(PortalViewModelStore portalViewModelStore) {
            _portalViewModelStore = portalViewModelStore;
        }

        protected override void Handle(PortalUndetectedEvent domainEvent) {
            var portalID  = domainEvent.ID;
            var viewModel = _portalViewModelStore.GetByID(portalID).AsViewModel();
            viewModel.SetFocus(false);
        }
    }
}