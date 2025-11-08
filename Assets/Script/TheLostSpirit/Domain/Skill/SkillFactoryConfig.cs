using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill
{
    [Serializable]
    public class SkillFactoryConfig
    {
        [SerializeField, AssetList(AutoPopulate = true)]
        SkillConfigWrapper[] skillConfigsWrapper;

        public SkillConfig[] SkillConfigs => skillConfigsWrapper.Select(wrapper => wrapper.Config).ToArray();
    }
}