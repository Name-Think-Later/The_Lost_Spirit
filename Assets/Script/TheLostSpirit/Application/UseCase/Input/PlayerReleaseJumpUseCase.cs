using TheLostSpirit.Domain.Player;

namespace TheLostSpirit.Application.UseCase.Input {
    public class PlayerReleaseJumpUseCase : IUseCase<Void, Void> {
        public Void Execute(Void input) {
            PlayerEntity.Get().ReleaseJump();

            return Void.Default;
        }
    }
}