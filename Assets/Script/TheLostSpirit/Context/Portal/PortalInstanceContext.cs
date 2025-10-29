using Script.TheLostSpirit.Application.ObjectContextContract;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;
using TheLostSpirit.Presentation.View;
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
        public IViewModelOnlyID<PortalID> ViewModelOnlyID { get; private set; }

        public PortalInstanceContext Construct() {
            var portalID = new PortalID();

            Entity = new PortalEntity(portalID, _mono);

            var viewModel = new PortalViewModel(portalID);

            ViewModelOnlyID = viewModel;

            _view.Bind(viewModel);

            return this;
        }
    }
}