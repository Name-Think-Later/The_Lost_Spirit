using MoreLinq;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Presentation.ViewModel.Portal;

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

            _portalViewModelStore.ForEach(portalViewModel => {
                var eachID  = portalViewModel.ID;
                var isFocus = eachID == portalID;

                portalViewModel.TransformToViewModel().SetFocus(isFocus);
            });
        }
    };
}