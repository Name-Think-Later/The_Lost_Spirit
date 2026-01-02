using MoreLinq;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Infrastructure.Domain.EntityMono;
using TheLostSpirit.Infrastructure.Domain.Specification;
using TheLostSpirit.Presentation.View;
using TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;
using UnityEngine;

namespace TheLostSpirit.Context.Formula
{
    public class ManifestationInstanceContext : MonoBehaviour, IManifestationInstanceContext
    {
        [SerializeField, Required, ChildGameObjectsOnly]
        ManifestationSpecification _specification;

        [SerializeField, Required, ChildGameObjectsOnly]
        ManifestationMono _mono;

        [SerializeField, Required, ChildGameObjectsOnly]
        ManifestationView _view;

        public ManifestationEntity Entity { get; private set; }
        public IViewModelReference<ManifestationID> ViewModelReference { get; private set; }

        public ManifestationInstanceContext Construct(
            FormulaPayload                     payload,
            ManifestationDoFrameActionsUseCase manifestationDoFrameActionsUseCase,
            ManifestationFinishUseCase         manifestationFinishUseCase
        ) {
            var manifestationID = ManifestationID.New();

            Entity = new ManifestationEntity(manifestationID, _specification.Config, _mono, payload);

            var viewModel =
                new ManifestationViewModel(
                    manifestationID,
                    manifestationDoFrameActionsUseCase,
                    manifestationFinishUseCase
                );

            ViewModelReference = viewModel;

            _view.Initialize(_specification.AnimationClip).Bind(viewModel);

            return this;
        }
    }
}