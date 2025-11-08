using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Core
{
    [CreateAssetMenu(fileName = "Core config", menuName = "The Lost Spirits/Core config")]
    public class CoreConfigWrapper : SkillConfigWrapper
    {
        [SerializeField, HideLabel]
        CoreConfig _coreConfig;

        public override SkillConfig Config => _coreConfig;
    }
}