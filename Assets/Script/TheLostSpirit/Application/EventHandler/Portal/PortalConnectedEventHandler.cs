using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Infrastructure.EventDriven;
using TheLostSpirit.ViewModel.Portal;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalConnectedEventHandler : DomainEventHandler<PortalConnectedEvent> {
        PortalRepository     _repository;
        PortalViewModelStore _viewModelStore;

        public PortalConnectedEventHandler(
            PortalRepository     repository,
            PortalViewModelStore viewModelStore
        ) {
            _repository     = repository;
            _viewModelStore = viewModelStore;
        }


        protected override void Handle(PortalConnectedEvent domainEvent) {
            var leftID  = domainEvent.LeftID;
            var rightID = domainEvent.RightID;

            var leftEntity  = _repository.GetByID(leftID);
            var rightEntity = _repository.GetByID(rightID);

            var leftViewModel  = _viewModelStore.GetByID(leftID);
            var rightViewModel = _viewModelStore.GetByID(rightID);

            leftViewModel.DebugLineDestination  = rightEntity.Position;
            rightViewModel.DebugLineDestination = leftEntity.Position;
        }
    }
}