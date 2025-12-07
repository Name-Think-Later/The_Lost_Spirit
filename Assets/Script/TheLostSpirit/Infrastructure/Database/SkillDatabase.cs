using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Identity.ConfigID;
using TheLostSpirit.Infrastructure.Domain.ConfigWrapper;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Database
{
    [CreateAssetMenu(fileName = "Skill Database", menuName = "The Lost Spirits/DataBase/Skill DataBase")]
    public class SkillDataBase
        : GenericDatabase<SkillConfigID, SkillConfig, SkillConfigWrapper>,
          ISkillDatabase
    { }
}