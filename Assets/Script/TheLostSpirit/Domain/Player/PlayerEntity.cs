using TheLostSpirit.Infrastructure.DomainDriven;
using UnityEngine;

namespace TheLostSpirit.Domain.Player {
    public class PlayerEntity : IEntity<PlayerID>, IPositionProvider {
        readonly Player      _player;
        readonly IPlayerMono _playerMono;

        public PlayerID ID { get; }

        public PlayerEntity(
            PlayerConfig config,
            IPlayerMono  playerMono
        ) {
            ID = new PlayerID();

            _player     = new Player(config);
            _playerMono = playerMono;

            playerMono.Initialize(ID);
        }

        public void MoveByAxis(int axis) {
            _player.Axis = axis;
            _playerMono.SetMoveSpeed(_player.FinalSpeed);
        }

        public void SetPosition(Vector2 position) {
            _playerMono.Transform.position = position;
        }

        public Vector2 GetPosition() {
            return _playerMono.Transform.position;
        }
    }
}