using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Player;

namespace TheLostSpirit.Application.UseCase.Player
{
    public class PlayerInteractUseCase : IUseCase<Void, Void>
    {
        public Void Execute(Void input) {
            PlayerEntity.Get().Interact();

            return Void.Default;
        }
    }
}