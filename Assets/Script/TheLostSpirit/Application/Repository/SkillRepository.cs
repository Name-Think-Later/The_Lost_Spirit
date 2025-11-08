using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Domain.Skill.Core;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.Repository
{
    public class SkillRepository : GenericRepository<SkillID, SkillEntity>
    {
        public CoreEntity GetByID(CoreID id) {
            return (CoreEntity)dictionary[id];
        }
    }
}