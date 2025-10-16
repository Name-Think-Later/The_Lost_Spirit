using TheLostSpirit.Domain.Player;
using TheLostSpirit.Infrastructure.UseCase;

namespace TheLostSpirit.Application.UseCase.Input {
    public class MoveInputUseCase : IUseCase<Void, MoveInputUseCase.Input> {
        public Void Execute(Input input) {
            var axis = input.Axis;
            PlayerEntity.Get().MoveByAxis(axis);

            return Void.Default;
        }

        public record struct Input(int Axis) : IInput;
    }
}