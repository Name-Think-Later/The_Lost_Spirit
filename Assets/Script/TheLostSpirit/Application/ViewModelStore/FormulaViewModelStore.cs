using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace TheLostSpirit.Application.ViewModelStore {
    public class FormulaViewModelStore : IViewModelStore<FormulaID>, IReadOnlyList<IViewModelOnlyID<FormulaID>> {
        readonly List<IViewModelOnlyID<FormulaID>> _list = new();

        public void Save(IViewModelOnlyID<FormulaID> viewModel) {
            _list.Add(viewModel);
        }

        public void Remove(FormulaID id) {
            var target = GetByID(id);
            _list.Remove(target);
        }

        public IViewModelOnlyID<FormulaID> GetByID(FormulaID id) {
            return _list.Find(viewModel => viewModel.ID == id);
        }

        public bool HasID(FormulaID id) {
            var target = GetByID(id);

            return _list.Contains(target);
        }

        public void Clear() {
            _list.Clear();
        }

        public IEnumerator<IViewModelOnlyID<FormulaID>> GetEnumerator() {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public int Count => _list.Count;

        public IViewModelOnlyID<FormulaID> this[int index] => _list[index];
    }
}