using Sirenix.OdinInspector;
using TheLostSpirit.Application.EventHandler.Portal;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Portal;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Context.Player;
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

            _ = new PortalDetectedEventHandler(PortalViewModelStore);
            _ = new PortalUndetectedEventHandler(PortalViewModelStore);
            _ = new PortalInteractedEventHandler(PortalRepository);
            _ = new PortalConnectedEventHandler(PortalRepository, PortalViewModelStore);

            CreateByInstanceUseCase = new CreatePortalByInstanceUseCase(PortalRepository, PortalViewModelStore);
            ConnectUseCase          = new ConnectPortalUseCase(PortalRepository);

            return this;
        }
    }
}