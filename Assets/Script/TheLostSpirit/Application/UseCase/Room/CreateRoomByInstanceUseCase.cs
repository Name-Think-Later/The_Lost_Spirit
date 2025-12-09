using MoreLinq;
using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Portal;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.UseCase.Room
{
    public class CreateRoomByInstanceUseCase
        : IUseCase<CreateRoomByInstanceUseCase.Output, CreateRoomByInstanceUseCase.Input>
    {
        readonly CreatePortalByInstanceUseCase _createPortalByInstanceUseCase;
        readonly RoomRepository                _roomRepository;
        readonly RoomViewModelStore            _roomViewModelStore;

        public CreateRoomByInstanceUseCase(
            RoomRepository                roomRepository,
            RoomViewModelStore            roomViewModelStore,
            CreatePortalByInstanceUseCase createPortalByInstanceUseCase
        ) {
            _roomRepository                = roomRepository;
            _roomViewModelStore            = roomViewModelStore;
            _createPortalByInstanceUseCase = createPortalByInstanceUseCase;
        }

        public Output Execute(Input input) {
            var roomEntity    = input.RoomInstanceContext.Entity;
            var roomViewModel = input.RoomInstanceContext.ViewModelReference;

            _roomRepository.Save(roomEntity);
            _roomViewModelStore.Save(roomViewModel);


            var portalObjectContexts = input.RoomInstanceContext.Portals;
            portalObjectContexts.ForEach(ctx => {
                var createPortalByInstanceInput  = new CreatePortalByInstanceUseCase.Input(ctx);
                var createPortalByInstanceOutput = _createPortalByInstanceUseCase.Execute(createPortalByInstanceInput);

                var portalID = createPortalByInstanceOutput.PortalID;
                roomEntity.IncludePortal(portalID);
            });

            var roomID = roomEntity.ID;

            return new Output(roomID);
        }

        public record struct Input(IRoomInstanceContext RoomInstanceContext) : IInput;

        public record struct Output(RoomID RoomID) : IOutput;
    }
}