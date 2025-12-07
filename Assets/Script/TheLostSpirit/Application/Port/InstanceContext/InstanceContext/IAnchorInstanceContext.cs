using TheLostSpirit.Domain.Skill.Anchor;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceContext
{
    public interface IAnchorInstanceContext
        : IInstanceContext<AnchorID, AnchorEntity, IViewModelReference<AnchorID>>
    { }
}