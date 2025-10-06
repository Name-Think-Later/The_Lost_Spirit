using MoreLinq;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Infrastructure.EventDriven;
using TheLostSpirit.ViewModel;

namespace TheLostSpirit.EntryPoint.Playground {
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
                var isFocus = pair.Key == portalID;
                pair.Value.SetFocus(isFocus);
            });
        }
    };
}