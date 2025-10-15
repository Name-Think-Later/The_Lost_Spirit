using TheLostSpirit.Infrastructure;
using UnityEngine;

namespace TheLostSpirit.Domain {
    public interface ITransformProvider {
        ReadOnlyTransform ReadOnlyTransform { get; }
    }
}