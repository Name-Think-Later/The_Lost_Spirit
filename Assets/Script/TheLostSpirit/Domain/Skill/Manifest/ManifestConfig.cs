using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    [Serializable]
    public class ManifestConfig : SkillConfig
    {
        public ManifestationSpecificationID ManifestationSpecificationID { get; private set; }

#if UNITY_EDITOR
        public void RelateManifestation(ManifestationSpecificationID id) {
            ManifestationSpecificationID = id;
        }
#endif
    }
}