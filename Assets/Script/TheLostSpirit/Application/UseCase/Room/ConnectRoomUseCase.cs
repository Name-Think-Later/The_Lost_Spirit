using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Portal;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.UseCase.Room
{
    public class ConnectRoomUseCase : IUseCase<Void, ConnectRoomUseCase.Input>
    {
        readonly RoomRepository       _roomRepository;
        readonly ConnectPortalUseCase _connectPortalUseCase;

        public ConnectRoomUseCase(
            RoomRepository       roomRepository,
            ConnectPortalUseCase connectPortalUseCase
        ) {
            _roomRepository       = roomRepository;
            _connectPortalUseCase = connectPortalUseCase;
        }

        public Void Execute(Input input) {
            var leftRoomID  = input.LeftRoomID;
            var rightRoomID = input.RightRoomID;

            var leftRoom  = _roomRepository.GetByID(leftRoomID);
            var rightRoom = _roomRepository.GetByID(rightRoomID);

            leftRoom.Associate(rightRoomID);
            rightRoom.Associate(leftRoomID);

            var leftPortalID  = input.LeftPortalID;
            var rightPortalID = input.RightPortalID;

            var connectPortalInput = new ConnectPortalUseCase.Input(leftPortalID, rightPortalID);
            _connectPortalUseCase.Execute(connectPortalInput);

            leftRoom.TurnPortalToUnavailable(leftPortalID);
            rightRoom.TurnPortalToUnavailable(rightPortalID);

            return Void.Default;
        }

        public record struct Input(
            RoomID   LeftRoomID,
            PortalID LeftPortalID,
            RoomID   RightRoomID,
            PortalID RightPortalID
        ) : IInput;

        public record struct Output(bool IsSuccess) : IOutput;
    }
}