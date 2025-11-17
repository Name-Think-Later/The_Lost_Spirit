using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TheLostSpirit.Domain.Formula.Event;
using TheLostSpirit.Domain.Formula.Node.Event;
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

        public NodeID CoreNode => _formula.CoreNode;

        public FormulaEntity(FormulaID id) {
            ID = id;

            _formula  = new Formula();
            _eventBus = AppScope.EventBus;
        }

        public void AddNode(NodeID node) {
            Contract.Assert(!_formula.Nodes.Contains(node), $"Node: {node} already in formula: {ID}");

            _formula.Nodes.Add(node);
        }

        public void AddCoreNode(NodeID node, CoreID coreID) {
            AddNode(node);
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
            _eventBus.Publish(new VisitedNodeEvent(CoreNode));
        }
    }
}