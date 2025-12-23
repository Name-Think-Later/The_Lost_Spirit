using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Domain.Skill
{
    public interface ISkillDatabase
    {
        public SkillConfig GetByID(SkillSpecificationID id);

        public bool HasID(SkillSpecificationID id);
    }
}