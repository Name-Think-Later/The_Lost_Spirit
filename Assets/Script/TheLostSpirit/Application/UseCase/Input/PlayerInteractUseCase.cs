using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Contract;
using TheLostSpirit.Domain.Player;

namespace TheLostSpirit.Application.UseCase.Input {
    public class PlayerInteractUseCase : IUseCase<Void, Void> {
        readonly InteractableRepository _interactableRepository;

        public PlayerInteractUseCase(
            InteractableRepository interactableRepository
        ) {
            _interactableRepository = interactableRepository;
        }

        public Void Execute(Void input) {
            var interactableID = PlayerEntity.Get().InteractableTarget;

            if (interactableID != null) {
                var interactable = _interactableRepository.GetByID(interactableID);
                interactable.Interacted();
            }

            return Void.Default;
        }
    }
}