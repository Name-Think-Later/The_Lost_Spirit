using System;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill
{
    [Serializable]
    public abstract class SkillConfig
    {
        [SerializeField]
        public int configID;

        [SerializeField]
        public string skillName;
    }
}