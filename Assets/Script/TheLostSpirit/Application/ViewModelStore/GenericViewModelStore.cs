using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Application.ViewModelStore
{
    public abstract class GenericViewModelStore<TId>
        : IViewModelStore<TId>, IEnumerable<IViewModelReference<TId>> where TId : IRuntimeID
    {
        protected readonly Dictionary<TId, IViewModelReference<TId>> dictionary =
            new Dictionary<TId, IViewModelReference<TId>>();

        public IEnumerator<IViewModelReference<TId>> GetEnumerator() {
            return dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Save(IViewModelReference<TId> viewModelReference) {
            dictionary[viewModelReference.ID] = viewModelReference;
        }

        public IViewModelReference<TId> GetByID(TId id) {
            return dictionary[id];
        }

        public void Remove(TId id) {
            dictionary.Remove(id);
        }

        public bool HasID(TId id) {
            return dictionary.ContainsKey(id);
        }

        public void Clear() {
            dictionary.Clear();
        }
    }
}