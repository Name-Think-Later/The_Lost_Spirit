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

        public (NodeID nodeID, CoreID coreID) CoreNode {
            get => _formula.HeadNode;
            set {
                _formula.HeadNode = value;
                var formulaAddedNode = new FormulaAddedCoreNodeEvent(ID, value.coreID);
                _eventBus.Publish(formulaAddedNode);
            }
        }

        public FormulaEntity(FormulaID id) {
            ID = id;

            _formula  = new Formula();
            _eventBus = AppScope.EventBus;
        }

        public void AddNode(NodeID node) {
            _formula.Nodes.Add(node);
        }

        public void RemoveNode(NodeID node) {
            _formula.Nodes.Remove(node);
        }

        public void ClearNode() {
            _formula.Nodes.Clear();
        }
    }
}