using Script.TheLostSpirit.MapSystem;
using UnityEngine;

namespace Script.TheLostSpirit.PlayerModule {
    public class InteractService {
        readonly Collider2D _collider;
        readonly LayerMask  _layerMask;

        IInteractableReference _focus;

        public InteractService(
            Collider2D collider,
            LayerMask  layerMask
        ) {
            _collider  = collider;
            _layerMask = layerMask;
        }
        
        
    }
}