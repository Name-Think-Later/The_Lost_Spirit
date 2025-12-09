using System.Linq;
using MoreLinq;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Room;
using TheLostSpirit.Domain.Util;
using TheLostSpirit.Extension.Linq;

namespace TheLostSpirit.Application.UseCase.Map
{
    public class GenerateMapUseCase : IUseCase<Void, GenerateMapUseCase.Input>
    {
        readonly ConnectRoomUseCase _connectRoomUseCase;
        readonly CreateRoomUseCase  _createRoomUseCase;
        readonly RoomRepository     _roomRepository;


        public GenerateMapUseCase(
            RoomRepository     roomRepository,
            CreateRoomUseCase  createRoomUseCase,
            ConnectRoomUseCase connectRoomUseCase
        ) {
            _roomRepository     = roomRepository;
            _createRoomUseCase  = createRoomUseCase;
            _connectRoomUseCase = connectRoomUseCase;
        }

        public Void Execute(Input input) {
            RoomCreation(input.Amount);

            MainConnection();

            BranchConnection();


            return Void.Default;
        }

        void RoomCreation(int amount) {
            for (var i = 0; i < amount; i++) _createRoomUseCase.Execute();
        }

        void MainConnection() {
            _roomRepository
                .Window(2)
                .ForEach(selector => {
                    var leftRoomEntity  = selector[0];
                    var rightRoomEntity = selector[1];

                    var leftPortalID  = leftRoomEntity.AvailablePortals.GetRandom();
                    var rightPortalID = rightRoomEntity.AvailablePortals.GetRandom();

                    var connectRoomInput =
                        new ConnectRoomUseCase.Input(
                            leftRoomEntity.ID,
                            leftPortalID,
                            rightRoomEntity.ID,
                            rightPortalID
                        );

                    _connectRoomUseCase.Execute(connectRoomInput);
                });
        }

        void BranchConnection() {
            _roomRepository.ForEach(leftRoomEntity => {
                var availablePortalCount = leftRoomEntity.AvailablePortals.Count();
                var branchAmount         = new CdfSampler(availablePortalCount + 1).Next();

                var tryGetLeftPortalID = leftRoomEntity.AvailablePortals.TakeRandom(branchAmount);

                tryGetLeftPortalID.ForEach(leftPortalID => {
                    var roomFilter =
                        _roomRepository
                            .Where(roomEntity => {
                                var notSameRoom = roomEntity != leftRoomEntity;
                                var notAssociate =
                                    !(roomEntity.IsAssociate(leftRoomEntity.ID) ||
                                      leftRoomEntity.IsAssociate(roomEntity.ID));

                                return notSameRoom && notAssociate;
                            })
                            .Shuffle()
                            .ToList();

                    if (!roomFilter.Any()) {
                        return;
                    }

                    roomFilter
                        .SkipUntil(rightRoomEntity => {
                            if (!rightRoomEntity.AvailablePortals.Any()) {
                                return false;
                            }

                            var rightPortalID = rightRoomEntity.AvailablePortals.GetRandom();

                            var connectRoomInput =
                                new ConnectRoomUseCase.Input(
                                    leftRoomEntity.ID,
                                    leftPortalID,
                                    rightRoomEntity.ID,
                                    rightPortalID
                                );

                            _connectRoomUseCase.Execute(connectRoomInput);

                            return true;
                        })
                        .Consume();
                });
            });
        }

        public record struct Input(int Amount) : IInput;
    }
}