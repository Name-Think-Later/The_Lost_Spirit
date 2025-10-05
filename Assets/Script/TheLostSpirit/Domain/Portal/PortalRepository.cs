using System.Collections.Generic;
using TheLostSpirit.Infrastructure.DomainDriven;

namespace TheLostSpirit.Domain.Portal {
    public class PortalRepository : IRepository<PortalID, PortalEntity> {
        readonly Dictionary<PortalID, PortalEntity> _repository = new();

        public void Add(PortalEntity portalEntity) {
            var id = portalEntity.ID;
            _repository[id] = portalEntity;
        }

        public void Remove(PortalID id) {
            _repository.Remove(id);
        }

        public PortalEntity GetByID(PortalID id) {
            return _repository[id];
        }

        public bool HasID(PortalID id) {
            return _repository.ContainsKey(id);
        }
    }
}