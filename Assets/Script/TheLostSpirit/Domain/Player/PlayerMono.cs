using Extension.Unity;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace TheLostSpirit.Domain.Player {
    public class PlayerMono : MonoBehaviour, IPlayerMono {
        [SerializeField, Required, ChildGameObjectsOnly]
        Rigidbody2D _rigidbody;

        [SerializeField, Required, ChildGameObjectsOnly]
        Collider2D _collider;

        [SerializeField, Required, ChildGameObjectsOnly]
        Collider2D _interactDetector;


        float           _moveSpeed;
        ContactFilter2D _filter;

        public PlayerID ID { get; private set; }
        public Transform Transform => transform;

        public void Initialize(PlayerID id) {
            ID = id;

            _filter = new ContactFilter2D {
                useTriggers  = true,
                useLayerMask = true,
                layerMask    = _interactDetector.includeLayers
            };

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