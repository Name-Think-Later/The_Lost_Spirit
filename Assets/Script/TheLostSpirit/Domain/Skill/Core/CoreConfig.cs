using System;
using LBG;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Core
{
    [Serializable]
    public class CoreConfig : SkillConfig
    {
        [SerializeField]
        public float handleCap;

        [SerializeField]
        public float recoverAmount;

        [SerializeField]
        public float recoverInterval;

        [SerializeField]
        public float rechargeCooldown;

        [SerializeReference, SubclassSelector]
        public IInputPolicy inputPolicy;

        [SerializeReference, SubclassSelector]
        public IOutputPolicy outputPolicy;

        public float HandleCap => handleCap;
        public float RecoverAmount => recoverAmount;
        public float RecoverInterval => recoverInterval;
        public float RechargeCooldown => rechargeCooldown;

        public IInputPolicy InputPolicy => inputPolicy;
        public IOutputPolicy OutputPolicy => outputPolicy;
    }
}