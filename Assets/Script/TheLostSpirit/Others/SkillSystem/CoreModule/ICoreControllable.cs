using UnityEngine.InputSystem;

namespace TheLostSpirit.Others.SkillSystem.CoreModule {
    public interface ICoreControllable {
        public InputAction GetActiveInput { get; }
        public void Activate();
    }
}