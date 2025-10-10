using TheLostSpirit.Domain.Player;

namespace TheLostSpirit.ViewModel {
    public class ReleaseJumpInputUseCase {
        readonly PlayerEntity _playerEntity;

        public ReleaseJumpInputUseCase(PlayerEntity playerEntity) {
            _playerEntity = playerEntity;
        }

        public void Execute() {
            _playerEntity.ReleaseJump();
        }
    }
}