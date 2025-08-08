using System;
using Script.TheLostSpirit.SkillSystem.SkillBase;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    partial class Core {
        [Serializable, HideLabel]
        public class Model {
            [SerializeField]
            Info _info;

            [SerializeField]
            Core.BehaviourData _behaviourData;

            public Info Info {
                get => _info;
                set => _info = value;
            }

            public BehaviourData BehaviourData {
                get => _behaviourData;
                set => _behaviourData = value;
            }
        }
    }
}