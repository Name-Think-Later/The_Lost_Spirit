using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Domain.Skill.Core;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.ConfigWrapper
{
    [CreateAssetMenu(fileName = "Core config", menuName = "The Lost Spirits/Core config")]
    public class CoreConfigWrapper : SkillConfigWrapper
    {
        [SerializeField, HideLabel]
        CoreConfig _coreConfig;

        public override SkillConfig Inner => _coreConfig;
    }
}