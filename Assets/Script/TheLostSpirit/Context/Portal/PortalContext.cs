using Sirenix.OdinInspector;
using TheLostSpirit.Application.EventHandler.Portal;
using TheLostSpirit.Application.UseCase.Portal;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Factory;
using TheLostSpirit.ViewModel.Portal;
using UnityEngine;
using UnityEngine.UI;

namespace TheLostSpirit.Context.Portal {
    public class PortalContext : MonoBehaviour {
        [SerializeField, AssetList]
        PortalObjectContext _portalObjectContext;

        public PortalRepository PortalRepository { get; private set; }
        public PortalViewModelStore PortalViewModelStore { get; private set; }
        public PortalFactory PortalFactory { get; private set; }

        public ConnectPortalUseCase ConnectPortalUseCase { get; private set; }

        public void Construct(
            PlayerObjectContext playerObjectContext
        ) {
            PortalRepository     = new PortalRepository();
            PortalViewModelStore = new PortalViewModelStore();
            PortalFactory        = new PortalFactory(_portalObjectContext, PortalRepository, PortalViewModelStore);

            var interactableRepository = playerObjectContext.InteractableRepository;

            _ = new PortalInRangeEventHandler(PortalRepository, interactableRepository);
            _ = new PortalOutOfRangeEventHandler(interactableRepository);
            _ = new PortalInFocusEventHandler(PortalViewModelStore);
            _ = new PortalOutOfFocusEventHandler(PortalViewModelStore);
            _ = new PortalTeleportEventHandler(PortalRepository);
            _ = new PortalConnectedEventHandler(PortalRepository, PortalViewModelStore);

            ConnectPortalUseCase = new ConnectPortalUseCase(PortalRepository);
        }
    }
}