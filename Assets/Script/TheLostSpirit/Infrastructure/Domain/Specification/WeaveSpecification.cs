using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Domain.Skill.Weave;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.Specification
{
    [CreateAssetMenu(fileName = "Weave Specification", menuName = "The Lost Spirits/Weave Specification")]

    public class WeaveSpecification : SkillSpecification
    {
        [SerializeField, HideLabel]
        WeaveConfig _weaveConfig;

        public override SkillConfig Config => _weaveConfig;
    }
}