using System;

namespace TheLostSpirit.Others.SkillSystem.CoreModule.OutputHandler {
    public class Once : IOutputHandler {
        public Action OutputAction { get; set; }

        public void HandleOutput() {
            OutputAction();
        }
    }
}