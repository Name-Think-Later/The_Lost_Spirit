using Script.TheLostSpirit.Application.General;
using Script.TheLostSpirit.Presentation.ViewModel.Formula;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Formula.Event;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class FormulaAddedNodeEventHandler : DomainEventHandler<FormulaAddedNodeEvent>
    {
        readonly NodeRepository        _nodeRepository;
        readonly SkillRepository       _skillRepository;
        readonly FormulaViewModelStore _formulaViewModelStore;

        public FormulaAddedNodeEventHandler(
            NodeRepository        nodeRepository,
            SkillRepository       skillRepository,
            FormulaViewModelStore formulaViewModelStore
        ) {
            _nodeRepository        = nodeRepository;
            _skillRepository       = skillRepository;
            _formulaViewModelStore = formulaViewModelStore;
        }

        protected override void Handle(FormulaAddedNodeEvent domainEvent) {
            var node = _nodeRepository.GetByID(domainEvent.NodeID);

            if (node.Skill is CoreID coreID) {
                var formulaViewModel = _formulaViewModelStore.GetByID(domainEvent.FormulaID).TransformToViewModel();
                var coreEntity       = _skillRepository.GetByID(coreID);
                var ioPolicy         = new FormulaIOPolicy(node.ID, coreEntity.InputPolicy, coreEntity.OutputPolicy);
                formulaViewModel.WithIOPolicy(ioPolicy);
            }
        }
    }
}