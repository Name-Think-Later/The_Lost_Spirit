using System.Linq;
using MoreLinq;
using TheLostSpirit.Application.ObjectFactoryContract;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Room;
using TheLostSpirit.Domain;

namespace TheLostSpirit.Application.UseCase.Map {
    public class GenerateMapUseCase : IUseCase<Void, GenerateMapUseCase.Input> {
        readonly IRoomFactory       _roomFactory;
        readonly RoomRepository     _roomRepository;
        readonly PortalRepository   _portalRepository;
        readonly ConnectRoomUseCase _connectRoomUseCase;

        public GenerateMapUseCase(
            IRoomFactory       roomFactory,
            RoomRepository     roomRepository,
            PortalRepository   portalRepository,
            ConnectRoomUseCase connectRoomUseCase
        ) {
            _roomFactory        = roomFactory;
            _roomRepository     = roomRepository;
            _portalRepository   = portalRepository;
            _connectRoomUseCase = connectRoomUseCase;
        }

        public Void Execute(Input input) {
            RoomCreation(input.Amount);

            MainConnection();

            BranchConnection();

            return Void.Default;
        }

        void RoomCreation(int amount) {
            _roomFactory.ResetCounter();

            for (int i = 0; i < amount; i++) {
                _roomFactory.CreateRandomAndRegister();
            }
        }

        public record struct Input(int Amount) : IInput;


        void MainConnection() {
            _roomRepository
                .Window(2)
                .ForEach(selector => {
                    var leftRoomEntity  = selector[0];
                    var rightRoomEntity = selector[1];

                    var leftPortalID  = leftRoomEntity.AvailablePortal.Shuffle().First();
                    var rightPortalID = rightRoomEntity.AvailablePortal.Shuffle().First();

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

                var tryGetLeftPortalID = leftRoomEntity.AvailablePortal.Shuffle().Take(branchAmount);

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

                            var rightPortalID = rightRoomEntity.AvailablePortal.Shuffle().First();

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