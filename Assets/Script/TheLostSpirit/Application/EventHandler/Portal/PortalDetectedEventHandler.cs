using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Presentation.ViewModel.Portal;

namespace TheLostSpirit.Application.EventHandler.Portal
{
    public class PortalDetectedEventHandler : DomainEventHandler<PortalDetectedEvent>
    {
        readonly PortalViewModelStore _portalViewModelStore;

        public PortalDetectedEventHandler(PortalViewModelStore portalViewModelStore) {
            _portalViewModelStore = portalViewModelStore;
        }

        protected override void Handle(PortalDetectedEvent domainEvent) {
            var portalID = domainEvent.ID;


            var viewModel = _portalViewModelStore.GetByID(portalID).AsViewModel();
            viewModel.SetFocus(true);
        }
    }
}