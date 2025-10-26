using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Domain;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.Repository
{
    public abstract class GenericRepository<TId, TEntity> : IRepository<TId, TEntity>, IEnumerable<TEntity>
        where TId : IIdentity where TEntity : IEntity<TId>
    {
        protected readonly Dictionary<TId, TEntity> dictionary = new();

        public void Save(TEntity entity) => dictionary[entity.ID] = entity;

        public void Remove(TId id) => dictionary.Remove(id);

        public TEntity GetByID(TId id) => dictionary[id];

        public bool HasID(TId id) => dictionary.ContainsKey(id);

        public void Clear() => dictionary.Clear();

        public IEnumerator<TEntity> GetEnumerator() => dictionary.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}