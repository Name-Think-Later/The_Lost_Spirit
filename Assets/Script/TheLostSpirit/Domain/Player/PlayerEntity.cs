using System.Diagnostics.Contracts;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Player
{
    public class PlayerEntity : IEntity<PlayerID>
    {
        readonly IEventBus _eventBus;

        readonly Player      _player;
        readonly IPlayerMono _playerMono;

        PlayerEntity(
            PlayerID     id,
            PlayerConfig config,
            IPlayerMono  mono
        ) {
            ID      = id;
            _player = new Player(config);

            _playerMono = mono;
            mono.Initialize(ID);
        }

        public IReadOnlyTransform ReadOnlyTransform => _playerMono.ReadOnlyTransform;

        public PlayerID ID { get; }


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

        public void Interact() {
            _playerMono.Interact();
        }

        public void SetPosition(Vector2 position) {
            _playerMono.SetPosition(position);
        }

        #region Static member

        static PlayerEntity _instance;

        public static PlayerEntity Construct(
            PlayerID     id,
            PlayerConfig config,
            IPlayerMono  mono,
            IEventBus    eventBus
        ) {
            _instance = new PlayerEntity(id, config, mono);

            return _instance;
        }


        public static PlayerEntity Get() {
            Contract.Assert(_instance != null, "PlayerEntity not created");

            return _instance;
        }

        #endregion
    }
}