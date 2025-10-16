using UnityEngine;

namespace TheLostSpirit.Infrastructure {
    public class ReadOnlyTransform {
        readonly Transform _transform;
        public ReadOnlyTransform(Transform transform) {
            _transform = transform;
        }

        public Vector3 Position => _transform.position;
        public Vector3 LocalScale => _transform.localScale;
        public Quaternion Rotation => _transform.rotation;
    }
}