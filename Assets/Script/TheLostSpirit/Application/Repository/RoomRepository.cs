using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.Repository {
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