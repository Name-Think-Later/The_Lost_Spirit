using System;
using Script.TheLostSpirit.SkillSystem.SkillBase;
using UnityEngine;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    public partial class Core : Skill {
        [Serializable]
        public class BehaviourData {
            
            [SerializeField]
            float _handleCap;

            [SerializeField]
            float _recoverAmount;

            [SerializeField]
            float _recoverInterval;

            [SerializeField]
            float _rechargeCooldown;
        }
    }
}