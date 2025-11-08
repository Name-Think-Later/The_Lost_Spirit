using MoreLinq;
using Script.TheLostSpirit.Application.Port.InstanceFactory;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;

namespace TheLostSpirit.Application.UseCase.Map
{
    public class ClearMapUseCase : IUseCase<Void, Void>
    {
        readonly RoomRepository     _roomRepository;
        readonly RoomViewModelStore _roomViewModelStore;
        readonly IRoomInstanceFactory _roomInstanceFactory;

        public ClearMapUseCase(
            RoomRepository     roomRepository,
            RoomViewModelStore roomViewModelStore,
            IRoomInstanceFactory roomInstanceFactory
        ) {
            _roomRepository     = roomRepository;
            _roomViewModelStore = roomViewModelStore;
            _roomInstanceFactory  = roomInstanceFactory;
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