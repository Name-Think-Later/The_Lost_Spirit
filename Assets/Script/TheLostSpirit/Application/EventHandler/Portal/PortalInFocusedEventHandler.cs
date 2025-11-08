using MoreLinq;
using Script.TheLostSpirit.Presentation.ViewModel.Portal;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Portal.Event;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalInFocusedEventHandler : DomainEventHandler<PortalInFocusedEvent> {
        readonly PortalViewModelStore _portalViewModelStore;

        public PortalInFocusedEventHandler(
            PortalViewModelStore portalViewModelStore
        ) {
            _portalViewModelStore = portalViewModelStore;
        }

        protected override void Handle(PortalInFocusedEvent domainEvent) {
            var portalID = domainEvent.ID;

            _portalViewModelStore.ForEach(viewModelOnlyID => {
                var eachID  = viewModelOnlyID.ID;
                var isFocus = eachID == portalID;

                viewModelOnlyID.TransformToViewModel().SetFocus(isFocus);
            });
        }
    };
}