using TheLostSpirit.Domain.Skill.Core;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Skill
{
    public class SkillFactory
    {
        readonly SkillFactoryConfig _factoryConfig;

        public SkillFactory(SkillFactoryConfig factoryConfig) {
            _factoryConfig = factoryConfig;
        }

        public SkillEntity Create(int configIndex) {
            var skillConfig = _factoryConfig.SkillConfigs[configIndex];

            return skillConfig switch {
                CoreConfig config => new CoreEntity(CoreID.New(), config),
                _                 => null
            };
        }
    }
}