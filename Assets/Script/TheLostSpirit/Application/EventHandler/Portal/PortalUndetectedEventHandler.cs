using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Presentation.ViewModel.Portal;
using TheLostSpirit.Presentation.ViewModel.ViewModelReference.ViewModelStore;

namespace TheLostSpirit.Application.EventHandler.Portal
{
    public class PortalUndetectedEventHandler : DomainEventHandler<PortalUndetectedEvent>
    {
        readonly PortalViewModelStore _portalViewModelStore;

        public PortalUndetectedEventHandler(PortalViewModelStore portalViewModelStore) {
            _portalViewModelStore = portalViewModelStore;
        }

        public override void Handle(PortalUndetectedEvent domainEvent) {
            var portalID  = domainEvent.ID;
            var viewModel = _portalViewModelStore.GetByID(portalID).AsViewModel();
            viewModel.SetFocus(false);
        }
    }
}