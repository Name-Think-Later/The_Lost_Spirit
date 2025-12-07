using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Domain.Skill.Manifest.Event;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Infrastructure.Domain.EntityMono;
using TheLostSpirit.Presentation.View;
using TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;
using UnityEngine;

public class ManifestationInstanceContext : MonoBehaviour, IManifestationInstanceContext
{
    [SerializeField]
    ManifestationMono _mono;

    [SerializeField]
    ManifestationView _view;

    public ManifestationEntity Entity { get; private set; }
    public IViewModelReference<ManifestationID> ViewModelReference { get; private set; }

    public ManifestationInstanceContext Construct() {
        var manifestationID = ManifestationID.New();

        Entity = new ManifestationEntity(manifestationID, _mono);

        var viewModel = new ManifestationViewModel(manifestationID);

        ViewModelReference = viewModel;

        _view.Bind(viewModel);

        return this;
    }
}