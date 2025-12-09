using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Domain;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.Repository
{
    public abstract class GenericRepository<TId, TEntity> : IRepository<TId, TEntity>, IEnumerable<TEntity>
        where TId : IRuntimeID
        where TEntity : IEntity<TId>
    {
        protected readonly Dictionary<TId, TEntity> dictionary = new Dictionary<TId, TEntity>();

        public bool IsEmpty => dictionary.Count == 0;

        public IEnumerator<TEntity> GetEnumerator() {
            return dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Save(TEntity entity) {
            dictionary[entity.ID] = entity;
        }

        public void Remove(TId id) {
            dictionary.Remove(id);
        }

        public TEntity GetByID(TId id) {
            return dictionary[id];
        }

        public bool HasID(TId id) {
            return dictionary.ContainsKey(id);
        }

        public void Clear() {
            dictionary.Clear();
        }

        public TEntity TakeByID(TId id) {
            var item = GetByID(id);
            Remove(id);

            return item;
        }
    }
}