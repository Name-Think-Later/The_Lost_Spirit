using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Identity.SpecificationID;
using TheLostSpirit.Infrastructure.Database;
using UnityEngine;

namespace TheLostSpirit.Context.Formula
{
    public class ManifestationInstanceFactory : IManifestationInstanceFactory
    {
        readonly ManifestationDatabase              _manifestationDatabase;
        readonly ManifestationDoFrameActionsUseCase _manifestationDoFrameActionsUseCase;
        readonly ManifestationFinishUseCase         _manifestationFinishUseCase;

        public ManifestationInstanceFactory(
            ManifestationDatabase              manifestationDatabase,
            ManifestationDoFrameActionsUseCase manifestationDoFrameActionsUseCase,
            ManifestationFinishUseCase         manifestationFinishUseCase
        ) {
            _manifestationDatabase              = manifestationDatabase;
            _manifestationDoFrameActionsUseCase = manifestationDoFrameActionsUseCase;
            _manifestationFinishUseCase         = manifestationFinishUseCase;
        }


        public IManifestationInstanceContext Create(
            ManifestationSpecificationID specificationID,
            FormulaPayload               payload,
            IReadOnlyTransform           anchorTransform
        ) {
            var specification = _manifestationDatabase.GetByID(specificationID);
            var instance =
                Object
                    .Instantiate(specification, anchorTransform.Position, Quaternion.identity)
                    .GetComponent<ManifestationInstanceContext>();
            instance
                .Construct(
                    payload,
                    _manifestationDoFrameActionsUseCase,
                    _manifestationFinishUseCase
                );

            return instance;
        }
    }
}