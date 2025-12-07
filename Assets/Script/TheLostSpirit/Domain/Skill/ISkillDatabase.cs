using TheLostSpirit.Identity.ConfigID;

namespace TheLostSpirit.Domain.Skill
{
    public interface ISkillDatabase
    {
        public SkillConfig GetByID(SkillConfigID id);

        public bool HasID(SkillConfigID id);
    }
}