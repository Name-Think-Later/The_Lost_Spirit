using System;
using TheLostSpirit.Application.UseCase.Contract;
using TheLostSpirit.Application.UseCase.Input;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Presentation.ViewModel.Player {
    public class PlayerViewModel : IViewModel<PlayerID> {
        readonly PlayerMoveUseCase        _playerMoveUseCase;
        readonly PlayerDoJumpUseCase      _playerDoJumpUseCase;
        readonly PlayerReleaseJumpUseCase _playerReleaseJumpUseCase;
        readonly PlayerInteractUseCase    _playerInteractUseCase;

        public PlayerID ID { get; }

        public PlayerViewModel(
            PlayerID                id,
            PlayerMoveUseCase        playerMoveUseCase,
            PlayerDoJumpUseCase      playerDoJumpUseCase,
            PlayerReleaseJumpUseCase playerReleaseJumpUseCase,
            PlayerInteractUseCase    playerInteractUseCase
        ) {
            ID = id;

            _playerMoveUseCase        = playerMoveUseCase;
            _playerDoJumpUseCase      = playerDoJumpUseCase;
            _playerReleaseJumpUseCase = playerReleaseJumpUseCase;
            _playerInteractUseCase    = playerInteractUseCase;
        }


        public void MoveInput(float value) {
            var axis = Math.Sign(value);

            var input = new PlayerMoveUseCase.Input(axis);
            _playerMoveUseCase.Execute(input);
        }

        public void DoJumpInput() {
            _playerDoJumpUseCase.Execute();
        }

        public void ReleaseJumpInput() {
            _playerReleaseJumpUseCase.Execute();
        }

        public void InteractInput() {
            _playerInteractUseCase.Execute();
        }
    }
}