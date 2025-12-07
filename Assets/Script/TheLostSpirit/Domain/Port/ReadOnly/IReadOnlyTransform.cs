using UnityEngine;

namespace TheLostSpirit.Domain.Port.ReadOnly
{
    public interface IReadOnlyTransform
    {
        public Vector3 Position { get; }
        public Vector3 LocalScale { get; }
        public Quaternion Rotation { get; }
    }
}