using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.Domain;

namespace TheLostSpirit.Domain.Room {
    public class RoomRepository : IRepository<RoomID, RoomEntity>, IEnumerable<RoomEntity> {
        Dictionary<RoomID, RoomEntity> _dictionary = new();


        public void Add(RoomEntity entity) {
            _dictionary[entity.ID] = entity;
        }

        public void Remove(RoomID id) {
            _dictionary.Remove(id);
        }

        public RoomEntity GetByID(RoomID id) {
            return _dictionary[id];
        }

        public bool HasID(RoomID id) {
            return _dictionary.ContainsKey(id);
        }

        public void Clear() {
            _dictionary.Clear();
        }

        public IEnumerator<RoomEntity> GetEnumerator() {
            return _dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}