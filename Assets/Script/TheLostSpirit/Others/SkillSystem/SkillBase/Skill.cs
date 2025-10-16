using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TheLostSpirit.Others.SkillSystem.SkillBase {
    public class Skill {
        public Skill(Info info) {
            GetInfo = info;
        }

        public Info GetInfo { get; }

        public virtual async UniTask Activate() {
            Debug.Log($"{GetInfo.Name}");
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }
    }
}