using TheLostSpirit.Domain.Player;
using TheLostSpirit.Infrastructure.UseCase;

namespace TheLostSpirit.Application.UseCase.Input {
    public class ReleaseJumpInputUseCase : IUseCase<Void, Void> {
        public Void Execute(Void input) {
            PlayerEntity.Get().ReleaseJump();

            return Void.Default;
        }
    }
}