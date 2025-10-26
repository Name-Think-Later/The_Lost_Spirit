using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Presentation.ViewModel.Portal;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalOutOfFocusEventHandler : DomainEventHandler<PortalOutOfFocusEvent> {
        readonly PortalViewModelStore _portalViewModelStore;

        public PortalOutOfFocusEventHandler(
            PortalViewModelStore portalViewModelStore
        ) {
            _portalViewModelStore = portalViewModelStore;
        }

        protected override void Handle(PortalOutOfFocusEvent domainEvent) {
            var portalID  = domainEvent.ID;
            var viewModel = _portalViewModelStore.GetByID(portalID).TransformToViewModel();
            viewModel.SetFocus(false);
        }
    }
}