using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Script.TheLostSpirit.SkillSystem.SkillBase {
    public partial class Skill {
        public Skill(Info info) {
            GetInfo = info;
        }

        public Info GetInfo { get; }

        public virtual async UniTask Activate() {
            Debug.Log($"{GetInfo.Name}");
            await UniTask.Delay(1000);
        }
    }
}