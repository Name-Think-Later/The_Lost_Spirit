using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Identity.ConfigID;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceFactory
{
    public interface IManifestationInstanceFactory
    {
        IManifestationInstanceContext Create(ManifestationConfigID configID, FormulaPayload payload);
    }
}