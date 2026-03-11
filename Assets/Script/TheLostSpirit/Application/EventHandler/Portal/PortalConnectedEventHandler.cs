using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Domain.Repository;
using TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Presentation.ViewModel.Portal;
using TheLostSpirit.Presentation.ViewModel.ViewModelReference.ViewModelStore;

namespace TheLostSpirit.Application.EventHandler.Portal
{
    public class PortalConnectedEventHandler : DomainEventHandler<PortalConnectedEvent>
    {
        readonly PortalRepository     _repository;
        readonly PortalViewModelStore _viewModelStore;

        public PortalConnectedEventHandler(
            PortalRepository     repository,
            PortalViewModelStore viewModelStore
        ) {
            _repository     = repository;
            _viewModelStore = viewModelStore;
        }


        public override void Handle(PortalConnectedEvent domainEvent) {
            var leftID  = domainEvent.LeftID;
            var rightID = domainEvent.RightID;

            var leftEntity  = _repository.GetByID(leftID);
            var rightEntity = _repository.GetByID(rightID);

            var leftViewModel  = _viewModelStore.GetByID(leftID).AsViewModel();
            var rightViewModel = _viewModelStore.GetByID(rightID).AsViewModel();

            //leftViewModel.DebugLineDestinationTarget  = rightEntity.ReadOnlyTransform;
            //rightViewModel.DebugLineDestinationTarget = leftEntity.ReadOnlyTransform;
        }
    }
}