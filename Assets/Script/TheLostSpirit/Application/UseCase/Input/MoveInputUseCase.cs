using TheLostSpirit.Domain.Player;

namespace Script.TheLostSpirit.Application.UseCase.Input {
    public class MoveInputUseCase {
        readonly PlayerEntity _playerEntity;

        public MoveInputUseCase(PlayerEntity playerEntity) {
            _playerEntity = playerEntity;
        }

        public void Execute(int axis) {
            _playerEntity.MoveByAxis(axis);
        }
    }
}