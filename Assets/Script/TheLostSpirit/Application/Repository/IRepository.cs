using TheLostSpirit.Domain;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.Repository {
    public interface IRepository<in T, U>
        where U : IEntity<T> where T : IIdentity {
        void Add(U entity);

        void Remove(T id);

        U GetByID(T id);

        bool HasID(T id);

        void Clear();
    }
}