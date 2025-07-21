using System;

namespace Script.TheLostSpirit.Circuit.SkillSystem.SkillBase {
    public partial class Skill {
        [Serializable]
        public class Info {
            public Info(string name) {
                Name = name;
            }

            public string Name { get; }
        }
    }
}