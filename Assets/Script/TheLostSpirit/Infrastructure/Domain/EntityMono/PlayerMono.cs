using System.Collections.Generic;
using R3;
using Sirenix.OdinInspector;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Extension.Unity;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.EntityMono
{
    public class PlayerMono : MonoBehaviour, IPlayerMono
    {
        [SerializeField, Required, ChildGameObjectsOnly]
        Rigidbody2D _rigidbody;

        [SerializeField, Required, ChildGameObjectsOnly]
        Collider2D _collider;

        [SerializeField, Required, ChildGameObjectsOnly]
        InteractableDetector _detector;

        IEventBus _eventBus;

        float _moveSpeed;
        float _originalGravityScale;


        public PlayerID ID { get; private set; }
        IRuntimeID IEntityMono.ID => ID;
        public IReadOnlyTransform ReadOnlyTransform { get; private set; }

        public void Initialize(PlayerID id) {
            ID        = id;
            _eventBus = AppScope.EventBus;

            ReadOnlyTransform = transform.ToReadOnly();

            FixedUpdateBinding();

            _detector.Initialize();
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

        public void SetPosition(Vector2 position) {
            transform.position = position;
        }

        public void RestoreGravityScale() {
            _rigidbody.gravityScale = _originalGravityScale;
        }

        public void Interact() {
            _detector.Target?.Interacted();
        }


        public void Destroy() {
            Destroy(gameObject);
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
    }
}