using UnityEngine;

namespace TheLostSpirit.Domain.Player {
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "TheLostSpirit/PlayerConfig")]
    public class PlayerConfig : ScriptableObject {
        public float baseSpeed = 10;

        public float baseJumpForce       = 10;
        public float jumpingGravityScale = 0.33f;
    }
}