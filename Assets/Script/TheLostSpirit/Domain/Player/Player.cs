namespace TheLostSpirit.Domain.Player
{
    public class Player
    {
        readonly PlayerConfig _config;


        public Player(PlayerConfig config) {
            _config = config;
        }

        public int Axis { get; set; }

        public float FinalSpeed => Axis * _config.baseSpeed;
        public float FinalJumpForce => _config.baseJumpForce;
        public float JumpingGravityScale => _config.jumpingGravityScale;
    }
}