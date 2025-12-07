using TheLostSpirit.Domain.Player;

namespace TheLostSpirit.Application.UseCase.Player
{
    public class PlayerDoJumpUseCase : IUseCase<Void, Void>
    {
        public Void Execute(Void input) {
            PlayerEntity.Get().DoJump();

            return Void.Default;
        }
    }
}