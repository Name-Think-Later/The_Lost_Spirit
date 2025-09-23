using R3;
using UnityEngine;

namespace Script.TheLostSpirit.MapSystem {
    public class PortalViewModel {
        readonly PortalController _portalController;

        public PortalViewModel(PortalController portalController) {
            _portalController = portalController;
        }

        public void OnInteracted(Transform target) {
            
            _portalController.Teleport(target);
        }
    }
}