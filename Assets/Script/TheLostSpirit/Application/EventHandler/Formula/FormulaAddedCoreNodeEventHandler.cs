using Script.TheLostSpirit.Presentation.ViewModel.Formula;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Formula.Event;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class FormulaAddedCoreNodeEventHandler : DomainEventHandler<FormulaAddedCoreNodeEvent>
    {
        readonly SkillRepository       _skillRepository;
        readonly FormulaViewModelStore _formulaViewModelStore;

        public FormulaAddedCoreNodeEventHandler(
            SkillRepository       skillRepository,
            FormulaViewModelStore formulaViewModelStore
        ) {
            _skillRepository       = skillRepository;
            _formulaViewModelStore = formulaViewModelStore;
        }

        protected override void Handle(FormulaAddedCoreNodeEvent domainEvent) {
            var formulaViewModel = _formulaViewModelStore.GetByID(domainEvent.FormulaID).TransformToViewModel();
            var coreEntity       = _skillRepository.GetByID(domainEvent.CoreID);
            formulaViewModel.WithIOPolicy(coreEntity.FormulaIOPolicy);
        }
    }
}