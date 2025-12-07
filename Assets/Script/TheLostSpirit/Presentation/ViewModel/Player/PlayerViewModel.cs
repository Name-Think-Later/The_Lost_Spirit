using System;
using System.Diagnostics.Contracts;
using TheLostSpirit.Application.UseCase;
using TheLostSpirit.Application.UseCase.Player;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Presentation.ViewModel.Player
{
    public class PlayerViewModel : IViewModel<PlayerID>
    {
        #region Static member

        static PlayerViewModel _instance;

        public static PlayerViewModel Get() {
            Contract.Assert(_instance != null, "PlayerViewModel not created");

            return _instance;
        }

        public static PlayerViewModel Construct(
            PlayerID                 playerID,
            PlayerMoveUseCase        playerMoveUseCase,
            PlayerDoJumpUseCase      playerDoJumpUseCase,
            PlayerReleaseJumpUseCase playerReleaseJumpUseCase,
            PlayerInteractUseCase    playerInteractUseCase
        ) {
            _instance =
                new PlayerViewModel(
                    playerID,
                    playerMoveUseCase,
                    playerDoJumpUseCase,
                    playerReleaseJumpUseCase,
                    playerInteractUseCase
                );

            return _instance;
        }

        #endregion

        readonly PlayerMoveUseCase        _playerMoveUseCase;
        readonly PlayerDoJumpUseCase      _playerDoJumpUseCase;
        readonly PlayerReleaseJumpUseCase _playerReleaseJumpUseCase;
        readonly PlayerInteractUseCase    _playerInteractUseCase;
        public PlayerID ID { get; }

        PlayerViewModel(
            PlayerID                 id,
            PlayerMoveUseCase        playerMoveUseCase,
            PlayerDoJumpUseCase      playerDoJumpUseCase,
            PlayerReleaseJumpUseCase playerReleaseJumpUseCase,
            PlayerInteractUseCase    playerInteractUseCase
        ) {
            ID                        = id;
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