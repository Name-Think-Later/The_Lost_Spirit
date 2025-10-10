using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using TheLostSpirit.FormulaSystem.NodeModule;
using TheLostSpirit.SkillSystem.CoreModule;
using UnityEngine.InputSystem;

namespace TheLostSpirit.FormulaSystem {
    public class Formula : ICollection<Node>, ICoreControllable {
        readonly Collection<Node> _circuit = new Collection<Node>();

        InputAction     _activeInput;
        SkillNode<Core> _head;
        public InputAction GetActiveInput => _activeInput;

        public void SetDefaultActiveInput(InputAction inputAction) {
            _activeInput = inputAction;
        }

        public void Activate() {
            _head.AsyncVisited().Forget();
        }


        public void Add(SkillNode<Core> coreSkillNode) {
            Add(coreSkillNode as Node);
            _head = coreSkillNode;
            _head.Skill.Initialize(this);
        }

        #region ICollection operator

        public int Count => _circuit.Count;
        public bool IsReadOnly => false;

        public IEnumerator<Node> GetEnumerator() => _circuit.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Node item) => _circuit.Add(item);

        public void Clear() => _circuit.Clear();

        public bool Contains(Node item) => _circuit.Contains(item);

        public void CopyTo(Node[] array, int arrayIndex) => _circuit.CopyTo(array, arrayIndex);

        public bool Remove(Node item) => _circuit.Remove(item);

        #endregion
    }
}