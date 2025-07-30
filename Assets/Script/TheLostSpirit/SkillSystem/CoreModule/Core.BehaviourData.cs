using System;
using UnityEngine;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    partial class Core {
        [Serializable]
        public partial class BehaviourData {
            [SerializeField]
            float _handleCap;

            [SerializeField]
            float _recoverAmount;

            [SerializeField]
            float _recoverInterval;

            [SerializeField]
            float _rechargeCooldown;

            [SerializeReference]
            ICircuitActivator _circuitActivator;

            public ICircuitActivator CircuitActivator {
                get => _circuitActivator;
                set => _circuitActivator = value;
            }
        }
    }
}