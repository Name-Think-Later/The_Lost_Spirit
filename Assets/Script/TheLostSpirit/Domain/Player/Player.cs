using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Player
{
    public class Player
    {
        readonly PlayerConfig _config;

        public int Axis { get; set; }
        
        public float FinalSpeed => Axis * _config.baseSpeed;
        public float FinalJumpForce => _config.baseJumpForce;
        public float JumpingGravityScale => _config.jumpingGravityScale;


        public Player(PlayerConfig config) {
            _config = config;
        }
    }
}