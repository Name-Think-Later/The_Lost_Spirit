using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using R3;
using ReactiveInputSystem;
using Script.TheLostSpirit.SkillSystem.CoreModule;
using UnityEngine.InputSystem;
using Core = Script.TheLostSpirit.SkillSystem.CoreModule.Core;

namespace Script.TheLostSpirit.CircuitSystem {
    public partial class Circuit : ICollection<Circuit.INode>, Core.ICoreControllable {
        readonly Collection<INode> _circuit;
        readonly InputAction       _activeInput;

        SkillNode<Core> _head;

        public Circuit(InputAction activeInput) {
            _circuit     = new Collection<INode>();
            _activeInput = activeInput;
        }

        public InputAction GetActiveInput() => _activeInput;

        public void Activate() {
            Traverse();
        }

        public void Traverse() {
            _head.AsyncActivate().Forget();
        }

        public void Add(SkillNode<Core> coreSkillNode) {
            Add(coreSkillNode as INode);
            _head = coreSkillNode;
            _head.Skill.Initialize(this);
        }

        #region ICollection operator

        public int Count => _circuit.Count;
        public bool IsReadOnly => false;

        public IEnumerator<INode> GetEnumerator() => _circuit.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(INode item) => _circuit.Add(item);

        public void Clear() => _circuit.Clear();

        public bool Contains(INode item) => _circuit.Contains(item);

        public void CopyTo(INode[] array, int arrayIndex) => _circuit.CopyTo(array, arrayIndex);

        public bool Remove(INode item) => _circuit.Remove(item);

        #endregion
    }
}