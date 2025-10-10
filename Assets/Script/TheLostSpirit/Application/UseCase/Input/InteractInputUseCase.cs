using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Player;

namespace TheLostSpirit.Application.UseCase.Input {
    public class InteractInputUseCase {
        readonly PlayerEntity           _playerEntity;
        readonly InteractableRepository _interactableRepository;

        public InteractInputUseCase(
            PlayerEntity           playerEntity,
            InteractableRepository interactableRepository
        ) {
            _playerEntity           = playerEntity;
            _interactableRepository = interactableRepository;
        }

        public void Execute() {
            var interactableID = _playerEntity.InteractableTarget;

            if (interactableID == null) return;
            var interactable = _interactableRepository.GetByID(interactableID);
            interactable.Interacted();
        }
    }
}