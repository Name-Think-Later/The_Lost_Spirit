using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Domain.Skill.Manifest;
using TheLostSpirit.Extension.General;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.Specification
{
    [CreateAssetMenu(fileName = "Manifest Specification", menuName = "The Lost Spirits/Manifest Specification")]
    public class ManifestSpecification : SkillSpecification
    {
        [SerializeField, HideLabel]
        ManifestConfig _manifestConfig;

        [Space(20)]
        [SerializeField, AssetList, InlineEditor(InlineEditorModes.FullEditor)]
        ManifestationSpecification _manifestationSpecification;

        public override SkillConfig Config => _manifestConfig;

#if UNITY_EDITOR
        void OnValidate() {
            Debug.Log($"{name}".Colored(Color.cyan) + " Update Manifestation Config ID");

            if (!_manifestationSpecification) return;

            var id = _manifestationSpecification.ID;
            _manifestConfig.RelateManifestation(id);
        }
#endif
    }
}