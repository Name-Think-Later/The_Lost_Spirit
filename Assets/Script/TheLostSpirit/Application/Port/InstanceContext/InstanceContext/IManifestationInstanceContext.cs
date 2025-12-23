using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceContext
{
    public interface IManifestationInstanceContext
        : IInstanceContext<ManifestationID, ManifestationEntity, IViewModelReference<ManifestationID>>
    { }
}