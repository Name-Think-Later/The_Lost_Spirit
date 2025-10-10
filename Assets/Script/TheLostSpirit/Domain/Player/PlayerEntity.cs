using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.DomainDriven;
using UnityEngine;

namespace TheLostSpirit.Domain.Player {
    public class PlayerEntity : IEntity<PlayerID>, IPositionProvider {
        readonly Player      _player;
        readonly IPlayerMono _playerMono;

        public PlayerID ID { get; }

        public IInteractableID InteractableTarget {
            get => _player.InteractableTarget;
            set => _player.InteractableTarget = value;
        }

        public Vector2 Position {
            get => _playerMono.Transform.position;
            set => _playerMono.Transform.position = value;
        }

        public PlayerEntity(
            PlayerID     id,
            PlayerConfig config,
            IPlayerMono  playerMono
        ) {
            ID = id;

            _player     = new Player(config);
            _playerMono = playerMono;

            playerMono.Initialize(ID);
        }

        public void MoveByAxis(int axis) {
            _player.Axis = axis;

            var finalSpeed = _player.FinalSpeed;
            _playerMono.SetMoveSpeed(finalSpeed);
        }

        public void DoJump() {
            var finalJumpForce      = _player.FinalJumpForce;
            var jumpingGravityScale = _player.JumpingGravityScale;
            _playerMono.Jump(finalJumpForce, jumpingGravityScale);
        }

        public void ReleaseJump() {
            _playerMono.RestoreGravityScale();
        }
    }
}