using System;
using Script.TheLostSpirit.Application.UseCase.Input;
using TheLostSpirit.Application.UseCase.Input;
using UnityEngine.InputSystem;

namespace TheLostSpirit.ViewModel {
    public class PlayerViewModel {
        readonly MoveInputUseCase     _moveInputUseCase;
        readonly InteractInputUseCase _interactInputUseCase;

        public PlayerViewModel(
            MoveInputUseCase     moveInputUseCase,
            InteractInputUseCase interactInputUseCase
        ) {
            _moveInputUseCase     = moveInputUseCase;
            _interactInputUseCase = interactInputUseCase;
        }

        public void AxisInput(InputAction.CallbackContext context) {
            var value = context.ReadValue<float>();
            var axis  = Math.Sign(value);
            _moveInputUseCase.Execute(axis);
        }

        public void InteractInput(InputAction.CallbackContext context) {
            _interactInputUseCase.Execute();
        }
    }
}