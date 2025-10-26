using TheLostSpirit.Application.UseCase.Contract;
using TheLostSpirit.Domain.Player;

namespace TheLostSpirit.Application.UseCase.Input {
    public class PlayerDoJumpUseCase : IUseCase<Void, Void> {
        public Void Execute(Void input) {
            PlayerEntity.Get().DoJump();

            return Void.Default;
        }
    }
}