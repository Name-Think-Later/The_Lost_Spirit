using R3;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Input;
using TheLostSpirit.Presentation.ViewModel;
using UnityEngine;

namespace TheLostSpirit.Context.Player
{
    public class PlayerContext : MonoBehaviour
    {
        public InteractableRepository InteractableRepository { get; private set; }

        public PlayerMoveUseCase PlayerMoveUseCase { get; private set; }
        public PlayerDoJumpUseCase PlayerDoJumpUseCase { get; private set; }
        public PlayerReleaseJumpUseCase PlayerReleaseJumpUseCase { get; private set; }
        public PlayerInteractUseCase PlayerInteractUseCase { get; private set; }

        public PlayerContext Construct() {
            InteractableRepository = new InteractableRepository();

            PlayerMoveUseCase        = new PlayerMoveUseCase();
            PlayerDoJumpUseCase      = new PlayerDoJumpUseCase();
            PlayerReleaseJumpUseCase = new PlayerReleaseJumpUseCase();
            PlayerInteractUseCase    = new PlayerInteractUseCase(InteractableRepository);
            
            return this;
        }
    }
}