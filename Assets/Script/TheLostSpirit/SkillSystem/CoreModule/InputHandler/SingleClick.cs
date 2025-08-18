using R3;
using ReactiveInputSystem;
using UnityEngine.InputSystem;

namespace Script.TheLostSpirit.SkillSystem.CoreModule.InputHandler {
    public class SingleClick : IInputHandler {
        public Observable<Unit> CreateObservableActivator(InputAction input) {
            return input.PerformedAsObservable().AsUnitObservable();
        }
    }
}