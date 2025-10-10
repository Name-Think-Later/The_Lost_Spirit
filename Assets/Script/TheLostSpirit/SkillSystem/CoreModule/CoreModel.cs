using System;
using Sirenix.OdinInspector;
using TheLostSpirit.SkillSystem.SkillBase;
using UnityEngine;

namespace TheLostSpirit.SkillSystem.CoreModule {
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