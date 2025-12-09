using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceFactory
{
    public interface IRoomInstanceFactory
    {
        IRoomInstanceContext Create();
        void Reset();
    }
}