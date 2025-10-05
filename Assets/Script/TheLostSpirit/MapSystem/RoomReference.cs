using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Portal;
using UnityEngine;
using ZLinq;

namespace TheLostSpirit.MapSystem {
    public class RoomReference : MonoBehaviour {
        [SerializeField, DisableIn(PrefabKind.All)]
        PortalMono[] _portalReferences;

        public PortalMono[] PortalReferences => _portalReferences;
        
#if UNITY_EDITOR
        
        
        [Button (ButtonSizes.Medium), DisableInPlayMode]
        void AutoGetPortals() {
            _portalReferences = transform.Children().OfComponent<PortalMono>().ToArray();
        }
#endif
    }
}