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


        public CreatePortalByInstanceUseCase CreatePortalByInstanceUseCase { get; private set; }
        public ConnectPortalUseCase ConnectPortalUseCase { get; private set; }

        public PortalContext Construct(PlayerContext playerContext) {
            PortalRepository     = new PortalRepository();
            PortalViewModelStore = new PortalViewModelStore();

            _ = new PortalTriggerEnteredEventHandler(PortalRepository, playerContext.InteractableRepository);
            _ = new PortalTriggerExitedEventHandler(playerContext.InteractableRepository);
            _ = new PortalInFocusedEventHandler(PortalViewModelStore);
            _ = new PortalOutOfFocusedEventHandler(PortalViewModelStore);
            _ = new PortalInteractedEventHandler(PortalRepository);
            _ = new PortalConnectedEventHandler(PortalRepository, PortalViewModelStore);

            CreatePortalByInstanceUseCase = new CreatePortalByInstanceUseCase(PortalRepository, PortalViewModelStore);
            ConnectPortalUseCase          = new ConnectPortalUseCase(PortalRepository);

            return this;
        }
    }
}