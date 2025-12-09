using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Domain.Skill.Core;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.Repository
{
    public class SkillRepository : GenericRepository<ISkillID, SkillEntity>
    {
        public CoreEntity GetByID(CoreID id) {
            return (CoreEntity)dictionary[id];
        }
    }
}