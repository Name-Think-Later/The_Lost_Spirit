using UnityEngine;

namespace TheLostSpirit.Domain.Player {
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "TheLostSpirit/PlayerConfig")]
    public class PlayerConfig : ScriptableObject {
        public float BaseSpeed;
    }
}