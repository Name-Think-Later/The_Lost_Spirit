using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Portal.Event;

namespace TheLostSpirit.Application.EventHandler.Portal {
    public class PortalConnectedEventHandler : DomainEventHandler<PortalConnectedEvent> {
        readonly PortalRepository     _repository;
        readonly PortalViewModelStore _viewModelStore;

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

            leftViewModel.DebugLineDestinationTarget  = rightEntity.ReadOnlyTransform;
            rightViewModel.DebugLineDestinationTarget = leftEntity.ReadOnlyTransform;
        }
    }
}