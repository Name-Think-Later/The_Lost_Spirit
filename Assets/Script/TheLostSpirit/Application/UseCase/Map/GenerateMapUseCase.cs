using System.Linq;
using Extension.Linq;
using MoreLinq;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Contract;
using TheLostSpirit.Application.UseCase.Room;
using TheLostSpirit.Domain;

namespace TheLostSpirit.Application.UseCase.Map
{
    public class GenerateMapUseCase : IUseCase<Void, GenerateMapUseCase.Input>
    {
        readonly RoomRepository     _roomRepository;
        readonly CreateRoomUseCase  _createRoomUseCase;
        readonly ConnectRoomUseCase _connectRoomUseCase;


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

        public record struct Input(int Amount) : IInput;

        void RoomCreation(int amount) {
            for (int i = 0; i < amount; i++) {
                _createRoomUseCase.Execute();
            }
        }

        void MainConnection() {
            _roomRepository
                .Window(2)
                .ForEach(selector => {
                    var leftRoomEntity  = selector[0];
                    var rightRoomEntity = selector[1];

                    var leftPortalID  = leftRoomEntity.AvailablePortal.GetRandom();
                    var rightPortalID = rightRoomEntity.AvailablePortal.GetRandom();

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
                var availablePortalCount = leftRoomEntity.AvailablePortal.Count;
                var branchAmount         = new CdfSampler(availablePortalCount + 1).Next();

                var tryGetLeftPortalID = leftRoomEntity.AvailablePortal.TakeRandom(branchAmount);

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

                    if (!roomFilter.Any()) return;

                    roomFilter
                        .SkipUntil(rightRoomEntity => {
                            if (!rightRoomEntity.AvailablePortal.Any()) return false;

                            var rightPortalID = rightRoomEntity.AvailablePortal.GetRandom();

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
    }
}