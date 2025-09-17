using System;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using ZLinq;

namespace Script.TheLostSpirit.MapSystem {
    public class RoomReference : MonoBehaviour {
        [SerializeField, DisableIn(PrefabKind.All)]
        PortalReference[] _portalReferences;

        public PortalReference[] PortalReferences => _portalReferences;
        
#if UNITY_EDITOR
        
        
        [Button (ButtonSizes.Medium), DisableInPlayMode]
        void AutoGetPortals() {
            _portalReferences = transform.Children().OfComponent<PortalReference>().ToArray();
        }
#endif
    }
}