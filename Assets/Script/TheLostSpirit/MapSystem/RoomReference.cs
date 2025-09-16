using System;
using Sirenix.OdinInspector;
using UnityEngine;
using ZLinq;

namespace Script.TheLostSpirit.MapSystem {
    public class RoomReference : MonoBehaviour {
        [OnInspectorGUI(nameof(AutoGetPortal))]
        [SerializeField, DisableIn(PrefabKind.All)]
        PortalReference[] _portalReferences;

        public PortalReference[] PortalReferences => _portalReferences;
        
        void AutoGetPortal() {
            _portalReferences = transform.Children().OfComponent<PortalReference>().ToArray();
        }
    }
}