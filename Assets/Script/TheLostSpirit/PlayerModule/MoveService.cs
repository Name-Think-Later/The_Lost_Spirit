using Script.Extension.Unity;
using UnityEngine;

namespace Script.TheLostSpirit.PlayerModule {
    public class MoveService {
        readonly Rigidbody2D _rigidbody;

        public MoveService(Rigidbody2D rigidbody) {
            _rigidbody = rigidbody;
        }

        public void ApplyVelocity(int axis, float speed) {
            var velocity = _rigidbody.velocity.WithX(axis * speed);
            _rigidbody.velocity = velocity;
        }
    }
}