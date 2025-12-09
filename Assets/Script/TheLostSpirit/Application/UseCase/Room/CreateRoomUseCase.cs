using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.UseCase.Room
{
    public class CreateRoomUseCase : IUseCase<CreateRoomUseCase.Output, Void>
    {
        readonly CreateRoomByInstanceUseCase _createRoomByInstanceUseCase;
        readonly IRoomInstanceFactory        _roomInstanceFactory;

        public CreateRoomUseCase(
            IRoomInstanceFactory        roomInstanceFactory,
            CreateRoomByInstanceUseCase createRoomByInstanceUseCase
        ) {
            _roomInstanceFactory         = roomInstanceFactory;
            _createRoomByInstanceUseCase = createRoomByInstanceUseCase;
        }

        public Output Execute(Void input) {
            var roomObjectContext = _roomInstanceFactory.Create();

            var createRoomByInstanceInput  = new CreateRoomByInstanceUseCase.Input(roomObjectContext);
            var createRoomByInstanceOutput = _createRoomByInstanceUseCase.Execute(createRoomByInstanceInput);

            var roomID = createRoomByInstanceOutput.RoomID;

            return new Output(roomID);
        }

        public record struct Output(RoomID ID) : IOutput;
    }
}