using System;
using Script.TheLostSpirit.Circuit.SkillSystem.SkillBase;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.TheLostSpirit.Circuit.SkillSystem.CoreModule {
    public partial class Core : Skill {
        [Serializable, HideLabel]
        public class Model {
            [SerializeField]
            Skill.Info _info;

            [SerializeField]
            Core.BehaviourData _behaviourData;

            public Skill.Info Info => _info;
            public Core.BehaviourData BehaviourData => _behaviourData;
        }
    }
}