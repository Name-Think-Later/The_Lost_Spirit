using System;
using Script.TheLostSpirit.SkillSystem.SkillBase;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    [Serializable, HideLabel]
    public class CoreModel {
        [SerializeField]
        Info _info;

        [SerializeField]
        CoreBehaviourData _behaviourData;

        public Info Info {
            get => _info;
            set => _info = value;
        }

        public CoreBehaviourData BehaviourData {
            get => _behaviourData;
            set => _behaviourData = value;
        }
    }
}