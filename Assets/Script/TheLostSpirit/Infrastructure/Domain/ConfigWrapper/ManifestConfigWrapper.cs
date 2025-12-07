using System.IO;
using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Domain.Skill.Manifest;
using TheLostSpirit.Extension.General;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.ConfigWrapper
{
    [CreateAssetMenu(fileName = "Manifest config", menuName = "The Lost Spirits/Manifest config")]
    public class ManifestConfigWrapper : SkillConfigWrapper
    {
        [SerializeField, HideLabel]
        ManifestConfig _manifestConfig;

        [Space(20)]
        [SerializeField, AssetList]
        [InlineEditor(InlineEditorModes.FullEditor)]
        ManifestationConfigWrapper _manifestationConfig;

        public override SkillConfig Inner => _manifestConfig;

#if UNITY_EDITOR
        void OnValidate() {
            Debug.Log($"{this.name}".Colored(Color.cyan) + " Update Manifestation Config ID");

            if (_manifestationConfig == null) return;

            var id = _manifestationConfig.ID;
            _manifestConfig.ManifestationConfigID = id;
        }
#endif
    }
}