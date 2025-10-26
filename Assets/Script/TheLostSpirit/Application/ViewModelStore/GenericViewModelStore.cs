using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace TheLostSpirit.Application.ViewModelStore
{
    public abstract class GenericViewModelStore<TId>
        : IViewModelStore<TId>, IEnumerable<IViewModelOnlyID<TId>> where TId : IIdentity
    {
        protected Dictionary<TId, IViewModelOnlyID<TId>> dictionary = new();

        public void Save(IViewModelOnlyID<TId> viewModelOnlyID) => dictionary[viewModelOnlyID.ID] = viewModelOnlyID;

        public IViewModelOnlyID<TId> GetByID(TId id) => dictionary[id];

        public void Remove(TId id) => dictionary.Remove(id);

        public bool HasID(TId id) => dictionary.ContainsKey(id);

        public void Clear() => dictionary.Clear();

        public IEnumerator<IViewModelOnlyID<TId>> GetEnumerator() => dictionary.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}