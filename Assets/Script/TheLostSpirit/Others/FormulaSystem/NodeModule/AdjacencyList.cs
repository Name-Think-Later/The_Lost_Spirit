using System.Collections;
using System.Collections.Generic;
using ZLinq;
using ZLinq.Linq;

namespace TheLostSpirit.Others.FormulaSystem.NodeModule {
    public class AdjacencyList : IList<Adjacency> {
        readonly List<Adjacency> _adjacencies = new List<Adjacency>();

        AdjacencyList(List<Adjacency> adjacencies) {
            _adjacencies = adjacencies;
        }

        public AdjacencyList(Node owner, int count) {
            for (int i = 0; i < count; i++) {
                _adjacencies.Add(new Adjacency(owner));
            }
        }

        public AdjacencyList In => _adjacencies.AsValueEnumerable().Where(a => a.IsIn).ToList();
        public AdjacencyList Out => _adjacencies.AsValueEnumerable().Where(a => a.IsOut).ToList();
        public AdjacencyList Exist => _adjacencies.AsValueEnumerable().Where(a => a.IsExist).ToList();
        public AdjacencyList Empty => _adjacencies.AsValueEnumerable().Where(a => a.IsEmpty).ToList();

        public bool Contains(Node node) => GetConnectedNodes().Contains(node);


        #region IList operation

        public int Count => _adjacencies.Count;
        public bool IsReadOnly => false;

        public IEnumerator<Adjacency> GetEnumerator() => _adjacencies.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Adjacency item) => _adjacencies.Add(item);

        public void Clear() => _adjacencies.Clear();

        public bool Contains(Adjacency item) => _adjacencies.Contains(item);

        public void CopyTo(Adjacency[] array, int arrayIndex) => _adjacencies.CopyTo(array, arrayIndex);

        public bool Remove(Adjacency item) {
            item.Cut();

            return _adjacencies.Remove(item);
        }

        public int IndexOf(Adjacency item) => _adjacencies.IndexOf(item);

        public void Insert(int index, Adjacency item) => _adjacencies.Insert(index, item);

        public void RemoveAt(int index) => Remove(_adjacencies[index]);

        public Adjacency this[int index] {
            get => _adjacencies[index];
            set => _adjacencies[index] = value;
        }

        #endregion

        public static implicit operator AdjacencyList(List<Adjacency> adjacencies) {
            return new AdjacencyList(adjacencies);
        }

        public ValueEnumerable<ListSelect<Adjacency, Node>, Node> GetConnectedNodes() =>
            _adjacencies.AsValueEnumerable().Select(a => a.Opposite.Owner);
    }
}