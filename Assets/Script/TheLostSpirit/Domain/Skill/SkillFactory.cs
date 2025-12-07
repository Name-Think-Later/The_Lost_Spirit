using TheLostSpirit.Domain.Skill.Core;
using TheLostSpirit.Domain.Skill.Manifest;
using TheLostSpirit.Identity.ConfigID;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill
{
    public class SkillFactory
    {
        readonly ISkillDatabase _skillDatabase;

        public SkillFactory(ISkillDatabase skillDatabase) {
            _skillDatabase = skillDatabase;
        }

        public SkillEntity Create(SkillConfigID configId) {
            var skillConfig = _skillDatabase.GetByID(configId);

            return skillConfig switch {
                CoreConfig config     => new CoreEntity(CoreID.New(), config),
                ManifestConfig config => new ManifestEntity(ManifestID.New(), config),
                _                     => null
            };
        }
    }
}