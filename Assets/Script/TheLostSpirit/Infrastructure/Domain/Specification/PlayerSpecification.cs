using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Player;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.Specification
{
    [CreateAssetMenu(fileName = "Player Specification", menuName = "The Lost Spirits/Player Specification")]
    public class PlayerSpecification : ScriptableObject
    {
        [SerializeField, HideLabel]
        PlayerConfig _playerConfig;


        public PlayerConfig Config => _playerConfig;
    }
}