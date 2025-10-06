using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Infrastructure.DomainDriven;

namespace TheLostSpirit.Infrastructure {
    public interface IViewModelStore<T, U> : IEnumerable<KeyValuePair<T, U>>
        where U : IViewModel<T> where T : IEntityID {
        void Add(U viewModel);

        void Remove(T id);

        U GetByID(T id);

        bool HasID(T id);
    }
}