using Script.TheLostSpirit.Application.ObjectContextContract;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;
using TheLostSpirit.Presentation.View;
using TheLostSpirit.Presentation.ViewModel.Portal;
using UnityEngine;

namespace TheLostSpirit.Context.Portal
{
    public class PortalObjectContext : MonoBehaviour, IPortalObjectContext
    {
        [SerializeField]
        PortalMono _mono;

        [SerializeField]
        PortalView _view;


        public PortalEntity Entity { get; private set; }
        public IViewModelOnlyID<PortalID> ViewModelOnlyID { get; private set; }

        public void Instantiate() {
            var id = new PortalID();

            Entity = new PortalEntity(id, _mono);

            var viewModel = new PortalViewModel(id);

            ViewModelOnlyID = viewModel;

            _view.Bind(viewModel);
        }
    }
}