using System;
using Script.TheLostSpirit.SkillSystem.SkillBase;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    public partial class Core : Skill {
        [Serializable, HideLabel]
        public class Model {
            [SerializeField]
            Skill.Info _info;

            [SerializeField]
            Core.BehaviourData _behaviourData;

            public Model(Skill.Info info, Core.BehaviourData behaviourData) {
                _info          = info;
                _behaviourData = behaviourData;
            }

            public Skill.Info Info => _info;
            public Core.BehaviourData BehaviourData => _behaviourData;
        }
    }
}