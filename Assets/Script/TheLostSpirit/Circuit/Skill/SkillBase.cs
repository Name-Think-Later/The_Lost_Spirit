using UnityEngine;

namespace Script.TheLostSpirit.Circuit.Skill {
    public class SkillBase {
        public SkillBase(SkillInfo info) {
            Info = info;
        }

        public SkillInfo Info { get; }

        public void Activate() {
            Debug.Log($"{Info.Name}");
        }
    }
}