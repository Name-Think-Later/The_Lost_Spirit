using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Skill.Manifest.Event;
using TheLostSpirit.Identity.ConfigID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.ConfigWrapper
{
    public class ManifestationConfigWrapper : MonoBehaviour, IConfigWrapper<ManifestationConfigID, ManifestationConfig>
    {
        [SerializeField, HideLabel]
        ManifestationConfig _config;

        public ManifestationConfigID ID => _config.ID;
        public ManifestationConfig Inner => _config;

        public bool IsMatch(string searchString) {
            var matchId = !string.IsNullOrEmpty(ID.Value) && ID.Value.Contains(searchString);

            return matchId;
        }
    }
}