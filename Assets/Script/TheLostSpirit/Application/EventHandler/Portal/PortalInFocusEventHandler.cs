using MoreLinq;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Infrastructure.EventDriven;
using TheLostSpirit.ViewModel.Portal;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalInFocusEventHandler : DomainEventHandler<PortalInFocusEvent> {
        readonly PortalViewModelStore _portalViewModelStore;

        public PortalInFocusEventHandler(
            PortalViewModelStore portalViewModelStore
        ) {
            _portalViewModelStore = portalViewModelStore;
        }

        protected override void Handle(PortalInFocusEvent domainEvent) {
            var portalID = domainEvent.ID;

            _portalViewModelStore.ForEach(pair => {
                var eachID  = pair.Key;
                var isFocus = eachID == portalID;

                var viewModel = pair.Value;
                viewModel.SetFocus(isFocus);
            });
        }
    };
}