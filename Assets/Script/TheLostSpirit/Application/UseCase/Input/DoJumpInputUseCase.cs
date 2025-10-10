using TheLostSpirit.Domain.Player;

namespace TheLostSpirit.ViewModel {
    public class DoJumpInputUseCase {
        readonly PlayerEntity _playerEntity;

        public DoJumpInputUseCase(PlayerEntity playerEntity) {
            _playerEntity = playerEntity;
        }

        public void Execute() {
            _playerEntity.DoJump();
        }
    }
}