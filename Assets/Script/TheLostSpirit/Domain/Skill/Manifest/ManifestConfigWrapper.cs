using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    public class ManifestConfigWrapper : SkillConfigWrapper
    {
        [SerializeField]
        ManifestConfig _manifestConfig;

        public override SkillConfig Config => _manifestConfig;
    }
}