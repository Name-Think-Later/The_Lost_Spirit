using Extension.Unity;
using R3;
using Sirenix.OdinInspector;
using TheLostSpirit.IDentify;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace TheLostSpirit.Domain.Player {
    public class PlayerMono : MonoBehaviour, IPlayerMono {
        [SerializeField, Required, ChildGameObjectsOnly]
        Rigidbody2D _rigidbody;

        [SerializeField, Required, ChildGameObjectsOnly]
        Collider2D _collider;

        float _moveSpeed;

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

        public void SetMoveSpeed(float x) {
            _moveSpeed = x;
        }

        public Vector2 GetPosition() {
            return transform.position;
        }

        void ApplyVelocity() {
            _rigidbody.velocity = _rigidbody.velocity.WithX(_moveSpeed);
        }
    }
}