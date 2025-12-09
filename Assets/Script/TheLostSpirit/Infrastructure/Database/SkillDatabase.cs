using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Identity.ConfigID;
using TheLostSpirit.Infrastructure.Domain.ConfigWrapper;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Database
{
    [CreateAssetMenu(fileName = "Skill Database", menuName = "The Lost Spirits/DataBase/Skill DataBase")]
    public class SkillDatabase
        : GenericDatabase<SkillConfigID, SkillConfig, SkillConfigWrapper>,
          ISkillDatabase
    {
        SkillConfig ISkillDatabase.GetByID(SkillConfigID id) {
            return GetByID(id).Inner;
        }
    }
}