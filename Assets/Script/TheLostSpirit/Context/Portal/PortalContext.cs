using R3;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.EventHandler.Portal;
using TheLostSpirit.Application.UseCase.Portal;
using TheLostSpirit.Context.Player;
using TheLostSpirit.Domain.Repository;
using TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Presentation.ViewModel.ViewModelReference.ViewModelStore;
using UnityEngine;

namespace TheLostSpirit.Context.Portal
{
    public class PortalContext : MonoBehaviour
    {
        [SerializeField, AssetList]
        PortalInstanceContext _portalInstanceContext;

        public PortalRepository PortalRepository { get; private set; }
        public PortalViewModelStore PortalViewModelStore { get; private set; }


        public CreatePortalByInstanceUseCase CreateByInstanceUseCase { get; private set; }
        public ConnectPortalUseCase ConnectUseCase { get; private set; }

        public PortalContext Construct(PlayerContext playerContext) {
            PortalRepository     = new PortalRepository();
            PortalViewModelStore = new PortalViewModelStore();

            _ = new PortalDetectedEventHandler(PortalViewModelStore).AddTo(this);
            _ = new PortalUndetectedEventHandler(PortalViewModelStore).AddTo(this);
            _ = new PortalInteractedEventHandler(PortalRepository).AddTo(this);
            _ = new PortalConnectedEventHandler(PortalRepository, PortalViewModelStore).AddTo(this);

            CreateByInstanceUseCase = new CreatePortalByInstanceUseCase(PortalRepository, PortalViewModelStore);
            ConnectUseCase          = new ConnectPortalUseCase(PortalRepository);

            return this;
        }
    }
}