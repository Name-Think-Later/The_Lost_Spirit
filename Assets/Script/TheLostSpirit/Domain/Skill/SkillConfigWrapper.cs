using UnityEngine;

namespace TheLostSpirit.Domain.Skill
{
    public abstract class SkillConfigWrapper : ScriptableObject
    {
        public abstract SkillConfig Config { get; }
    }
}