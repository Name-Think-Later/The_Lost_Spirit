using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Identity.ConfigID;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    [Serializable]
    public class ManifestConfig : SkillConfig
    {
        [ShowInInspector, HideReferenceObjectPicker]
        [ReadOnly, LabelText("Debug Manifestation Config")]
        public ManifestationConfigID ManifestationConfigID { get; set; }

        public ManifestConfig(SkillConfigID id) : base(id) { }
    }
}