using System;
using Script.TheLostSpirit.Exception;
using TheLostSpirit.Domain.Skill.Core;
using TheLostSpirit.Domain.Skill.Manifest;
using TheLostSpirit.Domain.Skill.Weave;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Domain.Skill
{
    public class SkillFactory
    {
        readonly ISkillDatabase _skillDatabase;

        public SkillFactory(ISkillDatabase skillDatabase) {
            _skillDatabase = skillDatabase;
        }

        public SkillEntity Create(SkillSpecificationID specificationID) {
            var skillConfig = _skillDatabase.GetByID(specificationID);

            return skillConfig switch {
                CoreConfig config     => new CoreEntity(CoreID.New(), config),
                ManifestConfig config => new ManifestEntity(ManifestID.New(), config),
                WeaveConfig config    => new WeaveEntity(WeaveID.New(), config),
                _                     => throw new UndefinedSkillTypeException()
            };
        }
    }
}