using Script.TheLostSpirit.Circuit.SkillSystem.SkillBase;

namespace Script.TheLostSpirit.Circuit.SkillSystem.CoreModule {
    public partial class Core : SkillBase.Skill {
        Core.Config _config;

        public Core(Skill.Info info, Core.Config config) : base(info) {
            _config = config;
        }
        
    }
}