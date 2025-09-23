using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.TheLostSpirit.PlayerModule {
    public class PlayerReference : MonoBehaviour {
        [SerializeField, Required, ChildGameObjectsOnly]
        Rigidbody2D _rigidbody;

        [SerializeField, Required, ChildGameObjectsOnly]
        Collider2D _collider;

        [SerializeField, Required, ChildGameObjectsOnly]
        Collider2D _interactDetector;

        [SerializeField]
        float _speed;

        public Rigidbody2D Rigidbody => _rigidbody;
        public Collider2D Collider => _collider;
        public float Speed => _speed;
    }
}