using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    [Serializable]
    public class ManifestConfig : SkillConfig
    {
        [ShowInInspector, HideReferenceObjectPicker]
        [ReadOnly, LabelText("Debug Manifestation Config")]

        public ManifestationSpecificationID ManifestationSpecificationID { get; private set; }

#if UNITY_EDITOR
        public void RelateManifestation(ManifestationSpecificationID id) {
            ManifestationSpecificationID = id;
        }
#endif
    }
}