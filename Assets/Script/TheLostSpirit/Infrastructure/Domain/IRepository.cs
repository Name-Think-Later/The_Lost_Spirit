using System.Collections.Generic;

namespace TheLostSpirit.Infrastructure.Domain {
    public interface IRepository<T, U> : IEnumerable<KeyValuePair<T, U>>
        where U : IEntity<T> where T : IIdentity {
        void Add(U entity);

        void Remove(T id);

        U GetByID(T id);

        bool HasID(T id);
    }
}