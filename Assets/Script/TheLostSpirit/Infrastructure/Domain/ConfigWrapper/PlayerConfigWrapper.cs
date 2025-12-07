using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Player;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.ConfigWrapper
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "The Lost Spirits/PlayerConfig")]
    public class PlayerConfigWrapper : ScriptableObject
    {
        [SerializeField, HideLabel]
        PlayerConfig _playerConfig;


        public PlayerConfig Inner => _playerConfig;
    }
}