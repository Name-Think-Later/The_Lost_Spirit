using System;
using R3;
using UnityEngine.InputSystem;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    public interface ICoreControllable {
        public InputAction GetActiveInput { get; }
        public void Activate();
    }
}