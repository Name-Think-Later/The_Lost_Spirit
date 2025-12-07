using System;

namespace TheLostSpirit.Domain.Player
{
    [Serializable]
    public class PlayerConfig
    {
        public float baseSpeed = 10;

        public float baseJumpForce       = 10;
        public float jumpingGravityScale = 0.33f;
    }
}