using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Skill
{
    public abstract class SkillEntity : IEntity<SkillID>
    {
        public SkillID ID { get; }

        protected SkillEntity(SkillID id) {
            ID = id;
        }

        public abstract void Activate();
    }
}