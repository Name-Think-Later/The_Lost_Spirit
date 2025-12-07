using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.Domain;
using TheLostSpirit.Infrastructure.Domain.EntityMono;
using TheLostSpirit.Presentation.View;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;
using TheLostSpirit.Presentation.ViewModel.Portal;
using UnityEngine;

namespace TheLostSpirit.Context.Portal
{
    public class PortalInstanceContext : MonoBehaviour, IPortalInstanceContext
    {
        [SerializeField]
        PortalMono _mono;

        [SerializeField]
        PortalView _view;


        public PortalEntity Entity { get; private set; }
        public IViewModelReference<PortalID> ViewModelReference { get; private set; }

        public PortalInstanceContext Construct() {
            var portalID = PortalID.New();

            Entity = new PortalEntity(portalID, _mono);

            var viewModel = new PortalViewModel(portalID);

            ViewModelReference = viewModel;

            _view.Bind(viewModel);

            return this;
        }
    }
}