using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using TheLostSpirit.Application.UseCase.Portal;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.UseCase;

namespace TheLostSpirit.Application.UseCase.Room {
    public class ConnectRoomUseCase : IUseCase<ConnectRoomUseCase.Output, ConnectRoomUseCase.Input> {
        readonly RoomRepository       _roomRepository;
        readonly PortalRepository     _portalRepository;
        readonly ConnectPortalUseCase _connectPortalUseCase;

        public ConnectRoomUseCase(
            RoomRepository       roomRepository,
            PortalRepository     portalRepository,
            ConnectPortalUseCase connectPortalUseCase
        ) {
            _roomRepository       = roomRepository;
            _portalRepository     = portalRepository;
            _connectPortalUseCase = connectPortalUseCase;
        }

        public Output Execute(Input input) {
            var success = Process(input);

            return new(success);
        }

        public record struct Input(RoomID LeftID, RoomID RightID) : IInput;

        public record struct Output(bool IsSuccess) : IOutput;

        bool Process(Input input) {
            var leftRoomID  = input.LeftID;
            var rightRoomID = input.RightID;

            var leftRoom  = _roomRepository.GetByID(leftRoomID);
            var rightRoom = _roomRepository.GetByID(rightRoomID);

            var usableLeftPortals = GetUsablePortals(leftRoom);

            if (!usableLeftPortals.Any()) return false;

            var usableRightPortals = GetUsablePortals(rightRoom);

            if (!usableRightPortals.Any()) return false;

            var leftPortalID  = usableLeftPortals[0].ID;
            var rightPortalID = usableRightPortals[0].ID;

            var connectPortalInput = new ConnectPortalUseCase.Input(leftPortalID, rightPortalID);
            _connectPortalUseCase.Execute(connectPortalInput);

            leftRoom.Associate(rightRoomID);
            rightRoom.Associate(leftRoomID);

            return true;
        }

        List<PortalEntity> GetUsablePortals(RoomEntity roomEntity) {
            return roomEntity
                   .IncludedPortal
                   .Select(id => _portalRepository.GetByID(id))
                   .Where(portal => !portal.HasAssociate)
                   .ToList();
        }
    }
}