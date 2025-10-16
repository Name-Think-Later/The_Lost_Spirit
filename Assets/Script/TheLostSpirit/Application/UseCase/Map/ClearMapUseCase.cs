using MoreLinq;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Infrastructure.UseCase;

namespace TheLostSpirit.Application.UseCase.Map {
    public class ClearMapUseCase : IUseCase<Void, Void> {
        readonly RoomRepository _roomRepository;

        public ClearMapUseCase(RoomRepository roomRepository) {
            _roomRepository = roomRepository;
        }

        public Void Execute(Void input) {
            _roomRepository.ForEach(entity => {
                entity.Dispose();
            });
            _roomRepository.Clear();

            return Void.Default;
        }
    }
}