using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identify;
using TheLostSpirit.View;
using TheLostSpirit.ViewModel.Portal;
using UnityEngine;

namespace TheLostSpirit.Context.Portal {
    public class PortalObjectContext : MonoBehaviour, IObjectContext<PortalID> {
        [SerializeField]
        PortalMono _mono;

        [SerializeField]
        PortalView _view;

        PortalRepository     _repository;
        PortalViewModelStore _viewModelStore;

        public void Construct(
            PortalRepository     repository,
            PortalViewModelStore viewModelStore
        ) {
            _repository     = repository;
            _viewModelStore = viewModelStore;
        }

        public PortalID Produce() {
            var id = new PortalID();

            var entity = new PortalEntity(id, _mono);
            _repository.Add(entity);

            var viewModel = new PortalViewModel(id);
            _viewModelStore.Add(viewModel);

            _view.Bind(viewModel);

            return id;
        }
    }
}