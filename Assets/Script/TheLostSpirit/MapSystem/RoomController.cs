using System.Collections.Generic;
using System.Linq;
using TheLostSpirit.Domain.Portal;
using UnityEngine;

namespace TheLostSpirit.MapSystem {
    public class RoomController {
        readonly RoomReference _roomReference;
        readonly List<Portal>  _portalControllers;

        public IEnumerable<Portal> ActivePortals => _portalControllers.Where(p => p.IsEnable);
        public IEnumerable<Portal> InactivePortals => _portalControllers.Where(p => !p.IsEnable);

        public RoomController(RoomReference roomReference) {
            _roomReference = roomReference;

            _portalControllers =
                _roomReference
                    .PortalReferences
                    .Select(p => new Portal())
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