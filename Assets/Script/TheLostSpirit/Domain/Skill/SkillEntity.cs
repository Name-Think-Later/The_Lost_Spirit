using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Skill
{
    public abstract class SkillEntity : IEntity<SkillID>
    {
        public SkillID ID { get; }

        public SkillEntity(SkillID id) {
            ID = id;
        }
    }
}