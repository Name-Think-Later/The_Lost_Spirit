using Script.TheLostSpirit.SkillSystem.SkillBase;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    public partial class Core : Skill {
        public interface ICoreControllable {
            public void CoreActivate();
        }
    }
}