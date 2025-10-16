using System;

namespace TheLostSpirit.Others.SkillSystem.CoreModule {
    public interface IOutputHandler {
        Action OutputAction { get; set; }
        public void HandleOutput();
    }
}