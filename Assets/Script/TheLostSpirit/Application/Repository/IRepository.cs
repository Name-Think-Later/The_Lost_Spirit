using TheLostSpirit.Domain;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.Repository
{
    public interface IRepository<in T, U>
        where U : IEntity<T> where T : IRuntimeID
    {
        void Save(U entity);

        void Remove(T id);

        U GetByID(T id);

        bool HasID(T id);

        void Clear();
    }
}