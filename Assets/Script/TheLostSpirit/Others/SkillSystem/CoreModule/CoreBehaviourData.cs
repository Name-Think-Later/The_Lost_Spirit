using System;
using LBG;
using UnityEngine;

namespace TheLostSpirit.Others.SkillSystem.CoreModule {
    [Serializable]
    public class CoreBehaviourData {
        [SerializeField]
        float _handleCap;

        [SerializeField]
        float _recoverAmount;

        [SerializeField]
        float _recoverInterval;

        [SerializeField]
        float _rechargeCooldown;

        [SerializeReference, SubclassSelector]
        IInputHandler _inputHandler;

        [SerializeReference, SubclassSelector(DrawClassFoldout = true)]
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