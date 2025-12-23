using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Identity.SpecificationID;
using TheLostSpirit.Infrastructure.Domain.Specification;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Database
{
    [CreateAssetMenu(fileName = "Skill Database", menuName = "The Lost Spirits/DataBase/Skill DataBase")]
    public class SkillDatabase
        : GenericDatabase<SkillSpecificationID, SkillSpecification>,
          ISkillDatabase
    {
        SkillConfig ISkillDatabase.GetByID(SkillSpecificationID id) {
            return GetByID(id).Config;
        }
    }
}