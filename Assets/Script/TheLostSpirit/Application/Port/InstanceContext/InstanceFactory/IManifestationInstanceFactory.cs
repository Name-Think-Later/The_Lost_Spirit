using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceFactory
{
    public interface IManifestationInstanceFactory
    {
        IManifestationInstanceContext Create(
            ManifestationSpecificationID specificationID,
            FormulaPayload               payload,
            IReadOnlyTransform           anchorTransform
        );
    }
}