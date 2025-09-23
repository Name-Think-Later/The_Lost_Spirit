using UnityEngine;

namespace Script.TheLostSpirit.PlayerModule {
    public class PlayerController {
        readonly PlayerReference _playerReference;
        readonly MoveService     _moveService;
        readonly InteractService _interactService;

        int _moveAxis;


        public PlayerController(
            PlayerReference playerReference
        ) {
            _playerReference = playerReference;
            _moveService     = new MoveService(_playerReference.Rigidbody);

            var layerMaskAll = 31;
            _interactService = new InteractService(_playerReference.Collider, layerMaskAll);
        }


        /// <summary>
        /// Remember to apply velocity
        /// </summary>
        public void SetAxis(int axis) {
            _moveAxis = axis;
        }

        public void UpdateVelocity() {
            _moveService.ApplyVelocity(_moveAxis, _playerReference.Speed);
        }


        public void SetInteractedTarget(Collider2D collider) {
        }


        public void Interact() { }
    }
}