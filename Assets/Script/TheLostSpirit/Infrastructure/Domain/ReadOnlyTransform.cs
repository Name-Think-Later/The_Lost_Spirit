using TheLostSpirit.Domain.Port.ReadOnly;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain
{
    public class ReadOnlyTransform : IReadOnlyTransform
    {
        readonly Transform _transform;

        public ReadOnlyTransform(Transform transform) {
            _transform = transform;
        }

        public Vector3 Position => _transform.position;
        public Vector3 LocalScale => _transform.localScale;
        public Quaternion Rotation => _transform.rotation;
    }

    public static class TransformExtension
    {
        public static IReadOnlyTransform ToReadOnly(this Transform transform) {
            return new ReadOnlyTransform(transform);
        }
    }
}