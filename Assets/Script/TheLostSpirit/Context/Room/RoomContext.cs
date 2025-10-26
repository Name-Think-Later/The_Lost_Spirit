using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Room;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Context.Portal;
using UnityEngine;

namespace TheLostSpirit.Context.Room
{
    public class RoomContext : MonoBehaviour
    {
        [SerializeField]
        RoomObjectFactoryConfig _config;

        public RoomRepository RoomRepository { get; private set; }
        public RoomViewModelStore RoomViewModelStore { get; private set; }
        public RoomObjectFactory RoomObjectFactory { get; private set; }


        public CreateRoomUseCase CreateRoomUseCase { get; private set; }
        public CreateRoomByInstanceUseCase CreateRoomByInstanceUseCase { get; private set; }
        public ConnectRoomUseCase ConnectRoomUseCase { get; private set; }

        public void Construct(PortalContext portalContext) {
            RoomRepository     = new RoomRepository();
            RoomViewModelStore = new RoomViewModelStore();
            RoomObjectFactory  = new RoomObjectFactory(_config);

            var portalRepository     = portalContext.PortalRepository;
            var portalViewModelStore = portalContext.PortalViewModelStore;

            CreateRoomByInstanceUseCase
                = new CreateRoomByInstanceUseCase(
                    RoomRepository,
                    RoomViewModelStore,
                    portalContext.CreatePortalByInstanceUseCase
                );


            CreateRoomUseCase =
                new CreateRoomUseCase(
                    RoomObjectFactory,
                    CreateRoomByInstanceUseCase
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