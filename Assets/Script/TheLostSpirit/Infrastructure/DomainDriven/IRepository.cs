namespace TheLostSpirit.Infrastructure.DomainDriven {
    public interface IRepository<in T, U> where U : IEntity<T> where T : IEntityID {
        void Add(U entity);

        void Remove(T id);

        U GetByID(T id);

        bool HasID(T id);
    }
}