using System.Collections.Generic;
using TheLostSpirit.Domain.Formula.Event;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Formula
{
    public class FormulaEntity : IEntity<FormulaID>
    {
        readonly Formula  _formula;
        readonly EventBus _eventBus;
        public FormulaID ID { get; }

        public IEnumerable<NodeID> Nodes => _formula.Nodes;

        public FormulaEntity(FormulaID id) {
            ID = id;

            _formula  = new Formula();
            _eventBus = AppScope.EventBus;
        }

        public void AddNode(NodeID node) {
            _formula.Nodes.Add(node);

            var formulaAddedNode = new FormulaAddedNodeEvent(ID, node);
            _eventBus.Publish(formulaAddedNode);
        }

        public void RemoveNode(NodeID node) {
            _formula.Nodes.Remove(node);
        }

        public void ClearNode() {
            _formula.Nodes.Clear();
        }
    }
}