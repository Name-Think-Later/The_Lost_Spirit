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
            IInputHandler _inputHandler;

            [SerializeReference]
            IOutputHandler _outputHandler;

            public IInputHandler InputHandler {
                get => _inputHandler;
                set => _inputHandler = value;
            }

            public IOutputHandler OutputHandler {
                get => _outputHandler;
                set => _outputHandler = value;
            }
        }
    }
}