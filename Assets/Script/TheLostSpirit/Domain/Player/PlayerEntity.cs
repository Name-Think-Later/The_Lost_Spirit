using TheLostSpirit.Exception;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.Domain;

namespace TheLostSpirit.Domain.Player {
    public class PlayerEntity : IEntity<PlayerID>, ITransformProvider {
        #region Static member

        static PlayerEntity _instance;

        public static PlayerEntity Construct(
            PlayerID     id,
            PlayerConfig config,
            IPlayerMono  mono
        ) {
            _instance = new PlayerEntity(id, config, mono);

            return _instance;
        }


        public static PlayerEntity Get() {
            return _instance ?? throw new PlayerEntityNotCreatedException();
        }

        #endregion

        readonly Player      _player;
        readonly IPlayerMono _playerMono;
        public PlayerID ID { get; }

        public IInteractableID InteractableTarget {
            get => _player.InteractableTarget;
            set => _player.InteractableTarget = value;
        }

        public ReadOnlyTransform ReadOnlyTransform { get; private set; }

        PlayerEntity(
            PlayerID     id,
            PlayerConfig config,
            IPlayerMono  mono
        ) {
            ID      = id;
            _player = new Player(config);

            _playerMono = mono;
            mono.Initialize(ID);

            ReadOnlyTransform = new ReadOnlyTransform(_playerMono.Transform);
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

        public void SetPosition(ITransformProvider transformProvider) {
            _playerMono.Transform.position = transformProvider.ReadOnlyTransform.Position;
        }
    }
}