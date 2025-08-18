using System;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    public interface IOutputHandler {
        Action OutputAction { get; set; }
        public void HandleOutput();
    }
}