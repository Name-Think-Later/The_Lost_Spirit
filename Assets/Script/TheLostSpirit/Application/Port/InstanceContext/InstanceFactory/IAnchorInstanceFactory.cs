using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceFactory
{
    public interface IAnchorInstanceFactory : IInstanceFactory<AnchorID, IAnchorInstanceContext>
    { }
}