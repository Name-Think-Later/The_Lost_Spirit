using Script.TheLostSpirit.Presentation.ViewModel.Portal;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Portal.Event;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalOutOfFocusedEventHandler : DomainEventHandler<PortalOutOfFocusedEvent> {
        readonly PortalViewModelStore _portalViewModelStore;

        public PortalOutOfFocusedEventHandler(
            PortalViewModelStore portalViewModelStore
        ) {
            _portalViewModelStore = portalViewModelStore;
        }

        protected override void Handle(PortalOutOfFocusedEvent domainEvent) {
            var portalID  = domainEvent.ID;
            var viewModel = _portalViewModelStore.GetByID(portalID).TransformToViewModel();
            viewModel.SetFocus(false);
        }
    }
}