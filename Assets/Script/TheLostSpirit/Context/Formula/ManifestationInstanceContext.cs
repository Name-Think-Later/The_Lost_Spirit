using Sirenix.OdinInspector;
using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Infrastructure.Domain.ConfigWrapper;
using TheLostSpirit.Infrastructure.Domain.EntityMono;
using TheLostSpirit.Presentation.View;
using TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;
using UnityEngine;

namespace TheLostSpirit.Context.Formula
{
    public class ManifestationInstanceContext : MonoBehaviour, IManifestationInstanceContext
    {
        [SerializeField, Required, ChildGameObjectsOnly]
        ManifestationConfigWrapper _configWrapper;

        [SerializeField, Required, ChildGameObjectsOnly]
        ManifestationMono _mono;

        [SerializeField, Required, ChildGameObjectsOnly]
        ManifestationView _view;

        public ManifestationEntity Entity { get; private set; }
        public IViewModelReference<ManifestationID> ViewModelReference { get; private set; }

        public ManifestationInstanceContext Construct(FormulaPayload payload) {
            var manifestationID = ManifestationID.New();

            Entity = new ManifestationEntity(manifestationID, _mono);

            var viewModel = new ManifestationViewModel(manifestationID);

            ViewModelReference = viewModel;

            _view.Bind(viewModel);

            return this;
        }
    }
}