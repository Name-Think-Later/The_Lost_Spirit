using TheLostSpirit.Application.UseCase.Player;
using UnityEngine;

namespace TheLostSpirit.Context.Player
{
    public class PlayerContext : MonoBehaviour
    {
        public PlayerMoveUseCase PlayerMoveUseCase { get; private set; }
        public PlayerDoJumpUseCase PlayerDoJumpUseCase { get; private set; }
        public PlayerReleaseJumpUseCase PlayerReleaseJumpUseCase { get; private set; }
        public PlayerInteractUseCase PlayerInteractUseCase { get; private set; }

        public PlayerContext Construct() {
            PlayerMoveUseCase        = new PlayerMoveUseCase();
            PlayerDoJumpUseCase      = new PlayerDoJumpUseCase();
            PlayerReleaseJumpUseCase = new PlayerReleaseJumpUseCase();
            PlayerInteractUseCase    = new PlayerInteractUseCase();

            return this;
        }
    }
}