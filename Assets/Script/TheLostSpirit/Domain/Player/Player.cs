using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Player
{
    public class Player
    {
        readonly PlayerConfig _config;

        public int Axis { get; set; }
        public IInteractableID InteractableTarget { get; set; }


        public float FinalSpeed => Axis * _config.baseSpeed;
        public float FinalJumpForce => _config.baseJumpForce;
        public float JumpingGravityScale => _config.jumpingGravityScale;


        public Player(PlayerConfig config) {
            _config = config;
        }
    }
}