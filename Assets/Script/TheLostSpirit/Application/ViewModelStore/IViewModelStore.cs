using System.Collections.Generic;
using TheLostSpirit.Identify;
using TheLostSpirit.ViewModel;

namespace TheLostSpirit.Application.ViewModelStore {
    public interface IViewModelStore<T, U> : IEnumerable<KeyValuePair<T, U>>
        where U : IViewModel<T> where T : IIdentity {
        void Add(U viewModel);

        void Remove(T id);

        U GetByID(T id);

        bool HasID(T id);
    }
}