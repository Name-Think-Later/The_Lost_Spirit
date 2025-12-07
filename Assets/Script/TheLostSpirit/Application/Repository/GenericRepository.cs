using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheLostSpirit.Domain;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.Repository
{
    public abstract class GenericRepository<TId, TEntity> : IRepository<TId, TEntity>, IEnumerable<TEntity>
        where TId : IRuntimeID
        where TEntity : IEntity<TId>
    {
        protected readonly Dictionary<TId, TEntity> dictionary = new();

        public void Save(TEntity entity) => dictionary[entity.ID] = entity;

        public void Remove(TId id) => dictionary.Remove(id);

        public TEntity GetByID(TId id) => dictionary[id];

        public TEntity TakeByID(TId id) {
            var item = GetByID(id);
            Remove(id);

            return item;
        }

        public bool HasID(TId id) => dictionary.ContainsKey(id);

        public void Clear() => dictionary.Clear();

        public bool IsEmpty => dictionary.Count == 0;

        public IEnumerator<TEntity> GetEnumerator() => dictionary.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}