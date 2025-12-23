using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Domain.Skill.Core;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.Specification
{
    [CreateAssetMenu(fileName = "Core Specification", menuName = "The Lost Spirits/Core Specification")]
    public class CoreSpecification : SkillSpecification
    {
        [SerializeField, HideLabel]
        CoreConfig _coreConfig;

        public override SkillConfig Config => _coreConfig;
    }
}