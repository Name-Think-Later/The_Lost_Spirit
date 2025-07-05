using System.Collections;
using System.Collections.Generic;
using ZLinq;

namespace Script.Circuit {
    public partial class CircuitNode {
        public class AdjacencyList : IList<Adjacency> {
            readonly List<Adjacency> _adjacencies = new List<Adjacency>();

            private AdjacencyList(List<Adjacency> adjacencies) {
                _adjacencies = adjacencies;
            }

            public AdjacencyList(CircuitNode owner, int count) {
                for (int i = 0; i < count; i++) {
                    _adjacencies.Add(new Adjacency(owner));
                }
            }

            public AdjacencyList In => _adjacencies.Where(a => a.IsIn).ToList();
            public AdjacencyList Out => _adjacencies.Where(a => a.IsOut).ToList();
            public AdjacencyList Exist => _adjacencies.Where(a => a.IsExist).ToList();
            public AdjacencyList Empty => _adjacencies.Where(a => a.IsEmpty).ToList();
            public int Count => _adjacencies.Count;
            public bool IsReadOnly => false;

            public void Remove(CircuitNode node) {
                var adjacency = _adjacencies.Find(a => a.Opposite.Equals(node));
                adjacency.Set(null);
            }

            public int IndexOf(CircuitNode node) => _adjacencies.FindIndex(a => a.Opposite.Equals(node));

            public bool Contains(CircuitNode node) => _adjacencies.Select(a => a.Opposite).Contains(node);


            #region IList operation

            public IEnumerator<Adjacency> GetEnumerator() => _adjacencies.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void Add(Adjacency item) => _adjacencies.Add(item);

            public void Clear() => _adjacencies.Clear();

            public bool Contains(Adjacency item) => _adjacencies.Contains(item);

            public void CopyTo(Adjacency[] array, int arrayIndex) => _adjacencies.CopyTo(array, arrayIndex);

            public bool Remove(Adjacency item) => _adjacencies.Remove(item);


            public int IndexOf(Adjacency item) => _adjacencies.IndexOf(item);

            public void Insert(int index, Adjacency item) => _adjacencies.Insert(index, item);

            public void RemoveAt(int index) => _adjacencies.RemoveAt(index);

            public Adjacency this[int index] {
                get => _adjacencies[index];
                set => _adjacencies[index] = value;
            }

            #endregion

            public static implicit operator AdjacencyList(List<Adjacency> adjacencies) {
                return new AdjacencyList(adjacencies);
            }
        }
    }
}