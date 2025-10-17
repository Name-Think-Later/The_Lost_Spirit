using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;

namespace TheLostSpirit.Context {
    public interface IObjectContext<out T> where T : IIdentity {
        T Produce();
    }
}