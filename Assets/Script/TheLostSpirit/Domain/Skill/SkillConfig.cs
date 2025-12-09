using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Identity.ConfigID;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill
{
    [Serializable]
    public abstract class SkillConfig : IConfig<SkillConfigID>
    {
        [SerializeField, HideLabel]
        SkillConfigID _id;

        [SerializeField]
        public string name;

        protected SkillConfig(SkillConfigID id) {
            _id = id;
        }

        public SkillConfigID ID => _id;
    }
}