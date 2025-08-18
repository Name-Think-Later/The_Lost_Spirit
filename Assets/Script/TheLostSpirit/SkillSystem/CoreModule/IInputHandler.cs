using R3;
using UnityEngine.InputSystem;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    public interface IInputHandler {
        public Observable<Unit> CreateObservableActivator(InputAction input);
    }
}