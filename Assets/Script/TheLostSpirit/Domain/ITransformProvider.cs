using TheLostSpirit.Infrastructure;

namespace TheLostSpirit.Domain {
    public interface ITransformProvider {
        ReadOnlyTransform ReadOnlyTransform { get; }
    }
}