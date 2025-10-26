using MoreLinq;
using Script.TheLostSpirit.Application.ObjectContextContract.ObjectFactory;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Contract;
using TheLostSpirit.Application.ViewModelStore;

namespace TheLostSpirit.Application.UseCase.Map
{
    public class ClearMapUseCase : IUseCase<Void, Void>
    {
        readonly RoomRepository     _roomRepository;
        readonly RoomViewModelStore _roomViewModelStore;
        readonly IRoomObjectFactory _roomObjectFactory;

        public ClearMapUseCase(
            RoomRepository     roomRepository,
            RoomViewModelStore roomViewModelStore,
            IRoomObjectFactory roomObjectFactory
        ) {
            _roomRepository     = roomRepository;
            _roomViewModelStore = roomViewModelStore;
            _roomObjectFactory  = roomObjectFactory;
        }

        public Void Execute(Void input) {
            _roomRepository.ForEach(entity => entity.Dispose());
            _roomRepository.Clear();

            _roomViewModelStore.Clear();

            _roomObjectFactory.Reset();

            return Void.Default;
        }
    }
}