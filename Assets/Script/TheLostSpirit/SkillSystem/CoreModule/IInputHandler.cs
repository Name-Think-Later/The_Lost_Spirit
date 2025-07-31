using R3;
using UnityEngine.InputSystem;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    partial class Core {
        partial class BehaviourData {
            public interface IInputHandler {
                public Observable<Unit> CreateObservableActivator(InputAction input);
            }
        }
    }
}