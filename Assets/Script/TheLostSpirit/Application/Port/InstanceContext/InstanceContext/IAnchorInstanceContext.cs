using TheLostSpirit.Domain.Skill.Anchor;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceContext
{
    public interface IAnchorInstanceContext
        : IInstanceContext<AnchorID, AnchorEntity, IViewModelReference<AnchorID>>
    { }
}