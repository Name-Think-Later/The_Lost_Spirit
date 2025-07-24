using System.Collections;
using System.Collections.Generic;

namespace Script.TheLostSpirit.CircuitSystem {
    public partial class Circuit : IList<Circuit.INode> {
        readonly List<INode> _circuit = new List<INode>();

        #region IList operator

        public int Count => _circuit.Count;
        public bool IsReadOnly => false;

        public IEnumerator<INode> GetEnumerator() => _circuit.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(INode item) => _circuit.Add(item);

        public void Clear() => _circuit.Clear();

        public bool Contains(INode item) => _circuit.Contains(item);

        public void CopyTo(INode[] array, int arrayIndex) => _circuit.CopyTo(array, arrayIndex);

        public bool Remove(INode item) => _circuit.Remove(item);


        public int IndexOf(INode item) => _circuit.IndexOf(item);

        public void Insert(int index, INode item) => _circuit.Insert(index, item);

        public void RemoveAt(int index) => _circuit.RemoveAt(index);

        public INode this[int index] {
            get => _circuit[index];
            set => _circuit[index] = value;
        }

        #endregion 
    }
}