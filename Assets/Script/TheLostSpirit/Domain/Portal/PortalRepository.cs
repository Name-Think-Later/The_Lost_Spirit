using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.Domain;

namespace TheLostSpirit.Domain.Portal {
    public class PortalRepository : IRepository<PortalID, PortalEntity> {
        readonly Dictionary<PortalID, PortalEntity> _dictionary = new();

        public void Add(PortalEntity portalEntity) {
            var id = portalEntity.ID;
            _dictionary[id] = portalEntity;
        }

        public void Remove(PortalID id) {
            _dictionary.Remove(id);
        }

        public PortalEntity GetByID(PortalID id) {
            return _dictionary[id];
        }

        public bool HasID(PortalID id) {
            return _dictionary.ContainsKey(id);
        }

        public IEnumerator<KeyValuePair<PortalID, PortalEntity>> GetEnumerator() {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}