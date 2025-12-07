using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Identity.ConfigID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.ConfigWrapper
{
    public abstract class SkillConfigWrapper : ScriptableObject, IConfigWrapper<SkillConfigID, SkillConfig>
    {
        public SkillConfigID ID => Inner.ID;
        public abstract SkillConfig Inner { get; }

        public bool IsMatch(string searchString) {
            var matchId   = !string.IsNullOrEmpty(ID.Value) && ID.Value.Contains(searchString);
            var matchName = !string.IsNullOrEmpty(Inner.name) && Inner.name.Contains(searchString);

            return matchId || matchName;
        }
    }
}