using System;
using R3;
using UnityEngine.InputSystem;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    partial class Core{
        public interface ICoreControllable {
            public InputAction GetActiveInput();
            public IDisposable ApplyActivator(Observable<Unit> observable);
        }
    }
}