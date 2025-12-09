using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Identity.ConfigID;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    [Serializable]
    public class ManifestConfig : SkillConfig
    {
        public ManifestConfig(SkillConfigID id) : base(id) { }

        [ShowInInspector, HideReferenceObjectPicker, ReadOnly, LabelText("Debug Manifestation Config")]
        public ManifestationConfigID ManifestationConfigID { get; set; }
    }
}