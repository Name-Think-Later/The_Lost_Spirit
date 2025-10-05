namespace TheLostSpirit.Domain.Player {
    public class Player {
        readonly PlayerConfig _config;

        public int Axis { get; set; }

        public float FinalSpeed => Axis * _config.BaseSpeed;


        public Player(PlayerConfig config) {
            _config = config;
        }
    }
}