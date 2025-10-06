using System;
using TheLostSpirit.Application.UseCase.Input;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;
using UnityEngine.InputSystem;

namespace TheLostSpirit.ViewModel {
    public class PlayerViewModel : IViewModel<PlayerID> {
        readonly MoveInputUseCase     _moveInputUseCase;
        readonly InteractInputUseCase _interactInputUseCase;

        public PlayerViewModel(
            PlayerID             id,
            MoveInputUseCase     moveInputUseCase,
            InteractInputUseCase interactInputUseCase
        ) {
            ID = id;

            _moveInputUseCase     = moveInputUseCase;
            _interactInputUseCase = interactInputUseCase;
        }

        public PlayerID ID { get; }

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