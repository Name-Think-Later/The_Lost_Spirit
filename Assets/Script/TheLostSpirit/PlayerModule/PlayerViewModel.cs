using System;
using UnityEngine.InputSystem;

namespace Script.TheLostSpirit.PlayerModule {
    public class PlayerViewModel {
        readonly PlayerController _playerController;

        public PlayerViewModel(PlayerController playerController) {
            _playerController = playerController;
        }

        public void AxisInput(InputAction.CallbackContext context) {
            var value = context.ReadValue<float>();
            var axis  = Math.Sign(value);
            _playerController.SetAxis(axis);
        }

        public void InteractInput(InputAction.CallbackContext context) {
            _playerController.Interact();
        }
    }
}