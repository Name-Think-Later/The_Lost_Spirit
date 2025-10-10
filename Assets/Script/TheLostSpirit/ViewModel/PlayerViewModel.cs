using System;
using TheLostSpirit.Application.UseCase.Input;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;

namespace TheLostSpirit.ViewModel {
    public class PlayerViewModel : IViewModel<PlayerID> {
        readonly MoveInputUseCase        _moveInputUseCase;
        readonly DoJumpInputUseCase      _doJumpInputUseCase;
        readonly ReleaseJumpInputUseCase _releaseJumpInputUseCase;
        readonly InteractInputUseCase    _interactInputUseCase;

        public PlayerID ID { get; }

        public PlayerViewModel(
            PlayerID                id,
            MoveInputUseCase        moveInputUseCase,
            DoJumpInputUseCase      doJumpInputUseCase,
            ReleaseJumpInputUseCase releaseJumpInputUseCase,
            InteractInputUseCase    interactInputUseCase
        ) {
            ID = id;

            _moveInputUseCase        = moveInputUseCase;
            _doJumpInputUseCase      = doJumpInputUseCase;
            _releaseJumpInputUseCase = releaseJumpInputUseCase;
            _interactInputUseCase    = interactInputUseCase;
        }


        public void MoveInput(float value) {
            var axis = Math.Sign(value);
            _moveInputUseCase.Execute(axis);
        }

        public void DoJumpInput() {
            _doJumpInputUseCase.Execute();
        }

        public void ReleaseJumpInput() {
            _releaseJumpInputUseCase.Execute();
        }

        public void InteractInput() {
            _interactInputUseCase.Execute();
        }
    }
}