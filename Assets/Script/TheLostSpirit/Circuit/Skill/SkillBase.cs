using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Script.TheLostSpirit.Circuit.Skill {
    public class SkillBase {
        public SkillBase(SkillInfo info) {
            Info = info;
        }

        public SkillInfo Info { get; }

        public async UniTask Activate() {
            Debug.Log($"{Info.Name}");
            await UniTask.Delay(1000);
        }
    }
}