using Extension.Unity;
using R3;
using Sirenix.OdinInspector;
using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Domain.Player {
    public class PlayerMono : MonoBehaviour, IPlayerMono {
        [SerializeField, Required, ChildGameObjectsOnly]
        Rigidbody2D _rigidbody;

        [SerializeField, Required, ChildGameObjectsOnly]
        Collider2D _collider;

        float _moveSpeed;
        float _originalGravityScale;

        public PlayerID ID { get; private set; }
        public Transform Transform => transform;

        public void Initialize(PlayerID id) {
            ID = id;

            FixedUpdateBinding();
        }

        void FixedUpdateBinding() {
            var fixedUpdate =
                Observable.EveryUpdate(UnityFrameProvider.FixedUpdate);

            fixedUpdate
                .Subscribe(_ => {
                    ApplyVelocity();
                })
                .AddTo(this);
        }

        void ApplyVelocity() {
            _rigidbody.velocity = _rigidbody.velocity.WithX(_moveSpeed);
        }


        public void SetMoveSpeed(float speed) {
            _moveSpeed = speed;
        }

        public void Jump(float force, float jumpingGravityScale) {
            _originalGravityScale   = _rigidbody.gravityScale;
            _rigidbody.gravityScale = jumpingGravityScale;

            var vector = Vector2.up * force;
            _rigidbody.AddForce(vector, ForceMode2D.Impulse);
        }

        public void RestoreGravityScale() {
            _rigidbody.gravityScale = _originalGravityScale;
        }

        public void Destroy() => Destroy(gameObject);
    }
}