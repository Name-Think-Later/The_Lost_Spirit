using UnityEngine.InputSystem;

namespace TheLostSpirit.SkillSystem.CoreModule {
    public interface ICoreControllable {
        public InputAction GetActiveInput { get; }
        public void Activate();
    }
}