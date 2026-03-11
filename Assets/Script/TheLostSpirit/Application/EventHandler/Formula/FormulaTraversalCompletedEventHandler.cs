using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Formula;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class FormulaTraversalCompletedEventHandler : DomainEventHandler<FormulaTraversalCompletedEvent>
    {
        AnchorRepository _anchorRepository;

        public FormulaTraversalCompletedEventHandler(AnchorRepository anchorRepository) {
            _anchorRepository = anchorRepository;
        }

        public override void Handle(FormulaTraversalCompletedEvent domainEvent) {
            var formulaStreamID = domainEvent.FormulaStreamID;
            var anchorEntities  = _anchorRepository.GetManyByFormulaStream(formulaStreamID);
            foreach (var anchorEntity in anchorEntities) {
                _anchorRepository.Remove(anchorEntity.ID);
                anchorEntity.Destroy();
            }
        }
    }
}