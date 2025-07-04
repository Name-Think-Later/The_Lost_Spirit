using Script.TheLostSpirit.Controller.PlayerController;
using UnityEngine.InputSystem;

namespace Script.TheLostSpirit.InputModule {
    public class GeneralKeyMapActions : ActionMap.IGeneralKeymapActions {
        readonly PlayerController _playerController;

        public GeneralKeyMapActions(PlayerController playerController) {
            _playerController = playerController;
        }

        public void OnMove(InputAction.CallbackContext context) {
            if (context.performed || context.canceled) {
                var axis = context.ReadValue<float>();
                _playerController.SetAxis(axis);
            }
        
        }
    }
}