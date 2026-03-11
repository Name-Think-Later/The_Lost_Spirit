using TheLostSpirit.Domain.Formula.Event;
using TheLostSpirit.Domain.Repository;
using TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Presentation.ViewModel.Formula;
using TheLostSpirit.Presentation.ViewModel.ViewModelReference.ViewModelStore;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class FormulaAddedCoreNodeEventHandler : DomainEventHandler<FormulaAddedCoreNodeEvent>
    {
        readonly FormulaViewModelStore _formulaViewModelStore;
        readonly SkillRepository       _skillRepository;

        public FormulaAddedCoreNodeEventHandler(
            SkillRepository       skillRepository,
            FormulaViewModelStore formulaViewModelStore
        ) {
            _skillRepository       = skillRepository;
            _formulaViewModelStore = formulaViewModelStore;
        }

        public override void Handle(FormulaAddedCoreNodeEvent domainEvent) {
            var formulaViewModel = _formulaViewModelStore.GetByID(domainEvent.FormulaID).AsViewModel();
            var coreEntity       = _skillRepository.GetByID(domainEvent.CoreID);
            formulaViewModel.WithIOPolicy(coreEntity.FormulaIOPolicy);
        }
    }
}