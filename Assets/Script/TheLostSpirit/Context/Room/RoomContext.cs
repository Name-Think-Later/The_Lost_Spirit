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
        RoomInstanceFactoryConfig _config;

        public RoomRepository RoomRepository { get; private set; }
        public RoomViewModelStore RoomViewModelStore { get; private set; }
        public RoomInstanceFactory RoomInstanceFactory { get; private set; }


        public CreateRoomUseCase CreateRoomUseCase { get; private set; }
        public CreateRoomByInstanceUseCase CreateRoomByInstanceUseCase { get; private set; }
        public ConnectRoomUseCase ConnectRoomUseCase { get; private set; }

        public RoomContext Construct(PortalContext portalContext) {
            RoomRepository      = new RoomRepository();
            RoomViewModelStore  = new RoomViewModelStore();
            RoomInstanceFactory = new RoomInstanceFactory(_config);

            CreateRoomByInstanceUseCase
                = new CreateRoomByInstanceUseCase(
                    RoomRepository,
                    RoomViewModelStore,
                    portalContext.CreateByInstanceUseCase
                );


            CreateRoomUseCase =
                new CreateRoomUseCase(
                    RoomInstanceFactory,
                    CreateRoomByInstanceUseCase
                );

            ConnectRoomUseCase =
                new ConnectRoomUseCase(
                    RoomRepository,
                    portalContext.ConnectUseCase
                );

            return this;
        }
    }
}