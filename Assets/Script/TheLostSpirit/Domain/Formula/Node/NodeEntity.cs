using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using TheLostSpirit.Domain.Formula.Node.Event;
using TheLostSpirit.Domain.Port;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Extension.General;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Formula.Node
{
    public class NodeEntity : IEntity<NodeID>
    {
        readonly Node      _node;
        readonly IEventBus _eventBus;

        public NodeID ID { get; }

        public ISkillID Skill {
            get => _node.Skill;
            set => _node.Skill = value;
        }

        public IEnumerable<Neighbor?> NullNeighbors =>
            _node
                .Neighbors
                .Where(n => !n.HasValue);

        public IEnumerable<Neighbor> NotNullNeighbors =>
            _node
                .Neighbors
                .Where(n => n.HasValue)
                .Cast<Neighbor>();

        public IEnumerable<Neighbor> OutNeighbors =>
            NotNullNeighbors
                .Where(n => n.IsOut);

        public NodeEntity(NodeID id, int neighborCount) {
            ID = id;

            _node     = new Node(neighborCount);
            _eventBus = AppScope.EventBus;
        }

        public void Associate(int index, Neighbor neighbor) {
            Contract.Assert(index.InClosedInterval(0, _node.Neighbors.Count - 1), "Index is invalid");

            _node.Neighbors[index] = neighbor;
        }

        public void Cut(int index) {
            Contract.Assert(index.InClosedInterval(0, _node.Neighbors.Count - 1), "Index is invalid");

            _node.Neighbors[index] = null;
        }


        public UniTask MoveNext(
            FormulaPayload  payload,
            TraversalPolicy traversalPolicy = TraversalPolicy.Sequential
        ) {
            var count = OutNeighbors.Count();
            var tasks =
                OutNeighbors
                    .Select((neighbor, index) => {
                        var clone = payload.Clone();
                        clone.isLastChild = (count - 1) == index;

                        var visitedNode = new AsyncVisitedNodeEvent(neighbor.ID, clone);

                        return _eventBus.PublishAsync(visitedNode);
                    });


            var moveNextEventAwait =
                traversalPolicy switch {
                    TraversalPolicy.Sequential =>
                        tasks
                            .ToUniTaskAsyncEnumerable()
                            .ForEachAwaitAsync(async task => await task),

                    TraversalPolicy.Parallel => UniTask.WhenAll(tasks),
                    _                        => UniTask.CompletedTask
                };

            return moveNextEventAwait;
        }
    }
}