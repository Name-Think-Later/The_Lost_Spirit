using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Application.ViewModelStore
{
    public class FormulaViewModelStore : IViewModelStore<FormulaID>, IReadOnlyList<IViewModelReference<FormulaID>>
    {
        readonly List<IViewModelReference<FormulaID>> _list = new();

        public void Save(IViewModelReference<FormulaID> viewModel) {
            _list.Add(viewModel);
        }

        public void Remove(FormulaID id) {
            var target = GetByID(id);
            _list.Remove(target);
        }

        public IViewModelReference<FormulaID> GetByID(FormulaID id) {
            return _list.Find(viewModel => viewModel.ID == id);
        }

        public bool HasID(FormulaID id) {
            var target = GetByID(id);

            return _list.Contains(target);
        }

        public void Clear() {
            _list.Clear();
        }

        public IEnumerator<IViewModelReference<FormulaID>> GetEnumerator() {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public int Count => _list.Count;

        public IViewModelReference<FormulaID> this[int index] => _list[index];
    }
}