using Script.TheLostSpirit.Application.ObjectContextContract.ObjectFactory;
using TheLostSpirit.Application.UseCase.Contract;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.UseCase.Room
{
    public class CreateRoomUseCase : IUseCase<CreateRoomUseCase.Output, Void>
    {
        readonly IRoomObjectFactory          _roomObjectFactory;
        readonly CreateRoomByInstanceUseCase _createRoomByInstanceUseCase;

        public CreateRoomUseCase(
            IRoomObjectFactory          roomObjectFactory,
            CreateRoomByInstanceUseCase createRoomByInstanceUseCase
        ) {
            _roomObjectFactory           = roomObjectFactory;
            _createRoomByInstanceUseCase = createRoomByInstanceUseCase;
        }

        public Output Execute(Void input) {
            var roomObjectContext = _roomObjectFactory.Create();

            var createRoomByInstanceInput  = new CreateRoomByInstanceUseCase.Input(roomObjectContext);
            var createRoomByInstanceOutput = _createRoomByInstanceUseCase.Execute(createRoomByInstanceInput);

            var roomID = createRoomByInstanceOutput.RoomID;

            return new Output(roomID);
        }

        public record struct Output(RoomID ID) : IOutput;
    }
}