using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script.TheLostSpirit.MapSystem {
    public class RoomController {
        readonly RoomReference          _roomReference;
        readonly List<PortalController> _portalControllers;

        public IEnumerable<PortalController> ActivePortals => _portalControllers.Where(p => p.IsActive);
        public IEnumerable<PortalController> InactivePortals => _portalControllers.Where(p => !p.IsActive);

        public RoomController(RoomReference roomReference) {
            _roomReference = roomReference;

            _portalControllers =
                _roomReference
                    .PortalReferences
                    .Select(p => new PortalController(p, this))
                    .ToList();
        }

        public void SetPosition(Vector2 position) {
            _roomReference.transform.position = position;
        }

        public void Destroy() {
            Object.Destroy(_roomReference.gameObject);
        }
    }
}