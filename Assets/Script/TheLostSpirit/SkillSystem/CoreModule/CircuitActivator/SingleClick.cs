using R3;
using ReactiveInputSystem;
using UnityEngine.InputSystem;

namespace Script.TheLostSpirit.SkillSystem.CoreModule.CircuitActivator {
    public class SingleClick : Core.BehaviourData.ICircuitActivator {
        public Observable<Unit> GetObservableActivator(InputAction input) {
            return input.PerformedAsObservable().AsUnitObservable();
        }
    }
}