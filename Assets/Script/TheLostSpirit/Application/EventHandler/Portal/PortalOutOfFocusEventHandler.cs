using MoreLinq;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Infrastructure.EventDriven;
using TheLostSpirit.ViewModel;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalOutOfFocusEventHandler : DomainEventHandler<PortalOutOfFocusEvent> {
        readonly PortalViewModelStore _portalViewModelStore;

        public PortalOutOfFocusEventHandler(
            PortalViewModelStore portalViewModelStore
        ) {
            _portalViewModelStore = portalViewModelStore;
        }

        protected override void Handle(PortalOutOfFocusEvent domainEvent) {
            var portalID        = domainEvent.ID;
            var portalViewModel = _portalViewModelStore.GetByID(portalID);
            portalViewModel.SetFocus(false);
        }
    }
}