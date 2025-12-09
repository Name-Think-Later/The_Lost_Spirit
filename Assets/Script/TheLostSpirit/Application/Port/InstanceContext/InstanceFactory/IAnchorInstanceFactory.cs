using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceFactory
{
    public interface IAnchorInstanceFactory
    {
        IAnchorInstanceContext Create();
    }
}