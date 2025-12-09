using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TheLostSpirit.Domain.Formula.Event;
using TheLostSpirit.Domain.Formula.Node.Event;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Formula
{
    public class FormulaEntity : IEntity<FormulaID>
    {
        readonly IEventBus _eventBus;
        readonly Formula   _formula;

        public FormulaEntity(FormulaID id) {
            ID = id;

            _formula  = new Formula();
            _eventBus = AppScope.EventBus;
        }

        public IEnumerable<NodeID> Nodes => _formula.Nodes;

        public NodeID CoreNode => _formula.CoreNode;
        public FormulaID ID { get; }

        public void AddNode(NodeID node, ISkillID skill) {
            Contract.Assert(!_formula.Nodes.Contains(node), $"Node: {node} already in formula: {ID}");

            _formula.Nodes.Add(node);

            if (skill is not CoreID coreID) {
                return;
            }

            _formula.CoreNode = node;

            var formulaAddedCoreNode = new FormulaAddedCoreNodeEvent(ID, coreID);
            _eventBus.Publish(formulaAddedCoreNode);
        }

        public void RemoveNode(NodeID node) {
            _formula.Nodes.Remove(node);
        }

        public void ClearNode() {
            _formula.Nodes.Clear();
        }

        public void Traverse() {
            _eventBus.Publish(new AsyncVisitedNodeEvent(CoreNode));
        }
    }
}