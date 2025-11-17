using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Extension.General;
using MoreLinq;
using TheLostSpirit.Domain.Formula.Node.Event;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.EventDriven;
using UnityEngine;

namespace TheLostSpirit.Domain.Formula.Node
{
    public class NodeEntity : IEntity<NodeID>
    {
        readonly Node     _node;
        readonly EventBus _eventBus;

        public NodeID ID { get; }

        public SkillID Skill {
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

        public async UniTask MoveNext(TraversalPolicy traversalPolicy = TraversalPolicy.Sequential) {
            var tasks =
                OutNeighbors
                    .Select(neighbor => _eventBus.PublishAwait(new VisitedNodeEvent(neighbor.ID)));


            var finalTask =
                traversalPolicy switch {
                    TraversalPolicy.Sequential =>
                        tasks
                            .ToUniTaskAsyncEnumerable()
                            .ForEachAwaitAsync(async task => await task),

                    TraversalPolicy.Parallel => UniTask.WhenAll(tasks),
                    _                        => UniTask.CompletedTask
                };

            await finalTask;
        }
    }
}