using System;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    partial class Core {
        partial class BehaviourData {
            public interface IOutputHandler {
                Action OutputAction { get; set; }
                public void HandleOutput();
            }
        }
    }
}