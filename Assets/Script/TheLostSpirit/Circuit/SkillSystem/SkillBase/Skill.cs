using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Script.TheLostSpirit.Circuit.SkillSystem.SkillBase {
    public partial class Skill {
        public Skill(Skill.Info info) {
            GetInfo = info;
        }

        public Skill.Info GetInfo { get; }

        public async UniTask Activate() {
            Debug.Log($"{GetInfo.Name}");
            await UniTask.Delay(1000);
        }
    }
}