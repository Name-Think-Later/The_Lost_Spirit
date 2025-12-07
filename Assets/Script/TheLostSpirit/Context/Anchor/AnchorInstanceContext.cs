using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Domain.Skill.Anchor;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Infrastructure.Domain.EntityMono;
using TheLostSpirit.Presentation.View;
using TheLostSpirit.Presentation.ViewModel.Formula;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;
using UnityEngine;

namespace TheLostSpirit.Context.Anchor
{
    public class AnchorInstanceContext : MonoBehaviour, IAnchorInstanceContext
    {
        [SerializeField]
        AnchorMono _mono;

        [SerializeField]
        AnchorView _view;

        public AnchorEntity Entity { get; private set; }
        public IViewModelReference<AnchorID> ViewModelReference { get; private set; }

        public AnchorInstanceContext Construct() {
            var anchorID = AnchorID.New();

            Entity = new AnchorEntity(anchorID, _mono);

            var viewModel = new AnchorViewModel(anchorID);

            ViewModelReference = viewModel;

            _view.Bind(viewModel);

            return this;
        }
    }
}