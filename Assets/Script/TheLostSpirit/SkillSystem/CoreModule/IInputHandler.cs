using R3;
using UnityEngine.InputSystem;

namespace TheLostSpirit.SkillSystem.CoreModule {
    public interface IInputHandler {
        public Observable<Unit> CreateObservableActivator(InputAction input);
    }
}