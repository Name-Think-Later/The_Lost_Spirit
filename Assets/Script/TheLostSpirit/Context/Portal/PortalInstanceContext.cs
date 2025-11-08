using Script.TheLostSpirit.Application.Port.InstanceContext;
using Script.TheLostSpirit.Presentation.View;
using Script.TheLostSpirit.Presentation.ViewModel.Portal;
using Script.TheLostSpirit.Presentation.ViewModel.UseCasePort;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identify;
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
        public IViewModelOnlyID<PortalID> ViewModelOnlyID { get; private set; }

        public PortalInstanceContext Construct() {
            var portalID = PortalID.New();

            Entity = new PortalEntity(portalID, _mono);

            var viewModel = new PortalViewModel(portalID);

            ViewModelOnlyID = viewModel;

            _view.Bind(viewModel);

            return this;
        }
    }
}