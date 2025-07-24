using System.Collections;
using System.Collections.Generic;

namespace Script.TheLostSpirit.CircuitSystem {
    public partial class Circuit : IList<Circuit.Node> {
        readonly List<Node> _circuit = new List<Node>();

        #region IList operator

        public int Count => _circuit.Count;
        public bool IsReadOnly => false;

        public IEnumerator<Node> GetEnumerator() => _circuit.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Node item) => _circuit.Add(item);

        public void Clear() => _circuit.Clear();

        public bool Contains(Node item) => _circuit.Contains(item);

        public void CopyTo(Node[] array, int arrayIndex) => _circuit.CopyTo(array, arrayIndex);

        public bool Remove(Node item) => _circuit.Remove(item);


        public int IndexOf(Node item) => _circuit.IndexOf(item);

        public void Insert(int index, Node item) => _circuit.Insert(index, item);

        public void RemoveAt(int index) => _circuit.RemoveAt(index);

        public Node this[int index] {
            get => _circuit[index];
            set => _circuit[index] = value;
        }

        #endregion 
    }
}