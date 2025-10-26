using TheLostSpirit.Application.UseCase.Contract;
using TheLostSpirit.Domain.Player;

namespace TheLostSpirit.Application.UseCase.Input {
    public class PlayerMoveUseCase : IUseCase<Void, PlayerMoveUseCase.Input> {
        public Void Execute(Input input) {
            var axis = input.Axis;
            PlayerEntity.Get().MoveByAxis(axis);

            return Void.Default;
        }

        public record struct Input(int Axis) : IInput;
    }
}