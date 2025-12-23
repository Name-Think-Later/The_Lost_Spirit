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
    }
}