using TheLostSpirit.Application.UseCase.Room;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Factory;
using TheLostSpirit.ViewModel.Room;
using UnityEngine;

namespace TheLostSpirit.Context.Room {
    public class RoomContext : MonoBehaviour {
        [SerializeField]
        RoomFactoryConfig _config;

        public RoomRepository RoomRepository { get; private set; }
        public RoomViewModelStore RoomViewModelStore { get; private set; }
        public RoomFactory RoomFactory { get; private set; }

        public ConnectRoomUseCase ConnectRoomUseCase { get; private set; }

        public void Construct(
            PortalContext portalContext
        ) {
            RoomRepository     = new RoomRepository();
            RoomViewModelStore = new RoomViewModelStore();

            var portalRepository     = portalContext.PortalRepository;
            var portalViewModelStore = portalContext.PortalViewModelStore;

            RoomFactory = new RoomFactory(
                _config,
                RoomRepository,
                RoomViewModelStore,
                portalRepository,
                portalViewModelStore
            );

            ConnectRoomUseCase =
                new ConnectRoomUseCase(
                    RoomRepository,
                    portalContext.PortalRepository,
                    portalContext.ConnectPortalUseCase
                );
        }
    }
}