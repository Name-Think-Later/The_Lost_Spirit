namespace TheLostSpirit.Domain.Player
{
    public class Player
    {
        readonly PlayerConfig _config;


        public Player(PlayerConfig config) {
            _config = config;
        }

        public int Axis { get; set; }

        public float FinalSpeed => Axis * _config.BaseSpeed;
        public float FinalJumpForce => _config.BaseJumpForce;
        public float JumpingGravityScale => _config.JumpingGravityScale;
    }
}