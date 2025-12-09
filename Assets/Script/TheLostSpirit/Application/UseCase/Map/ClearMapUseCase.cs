using MoreLinq;
using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;

namespace TheLostSpirit.Application.UseCase.Map
{
    public class ClearMapUseCase : IUseCase<Void, Void>
    {
        readonly IRoomInstanceFactory _roomInstanceFactory;
        readonly RoomRepository       _roomRepository;
        readonly RoomViewModelStore   _roomViewModelStore;

        public ClearMapUseCase(
            RoomRepository       roomRepository,
            RoomViewModelStore   roomViewModelStore,
            IRoomInstanceFactory roomInstanceFactory
        ) {
            _roomRepository      = roomRepository;
            _roomViewModelStore  = roomViewModelStore;
            _roomInstanceFactory = roomInstanceFactory;
        }

        public Void Execute(Void input) {
            _roomRepository.ForEach(entity => entity.Dispose());
            _roomRepository.Clear();

            _roomViewModelStore.Clear();

            _roomInstanceFactory.Reset();

            return Void.Default;
        }
    }
}