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
        PortalObjectContext _portalObjectContext;

        public PortalRepository PortalRepository { get; private set; }
        public PortalViewModelStore PortalViewModelStore { get; private set; }


        public CreatePortalByInstanceUseCase CreatePortalByInstanceUseCase { get; private set; }
        public ConnectPortalUseCase ConnectPortalUseCase { get; private set; }

        public void Construct(PlayerObjectContext playerObjectContext) {
            PortalRepository     = new PortalRepository();
            PortalViewModelStore = new PortalViewModelStore();

            var interactableRepository = playerObjectContext.InteractableRepository;

            _ = new PortalInRangeEventHandler(PortalRepository, interactableRepository);
            _ = new PortalOutOfRangeEventHandler(interactableRepository);
            _ = new PortalInFocusEventHandler(PortalViewModelStore);
            _ = new PortalOutOfFocusEventHandler(PortalViewModelStore);
            _ = new PortalTeleportEventHandler(PortalRepository);
            _ = new PortalConnectedEventHandler(PortalRepository, PortalViewModelStore);

            CreatePortalByInstanceUseCase = new CreatePortalByInstanceUseCase(PortalRepository, PortalViewModelStore);
            ConnectPortalUseCase          = new ConnectPortalUseCase(PortalRepository);
        }
    }
}