using UnityEngine;

namespace Script.TheLostSpirit.MapSystem {
    public class PortalReference : MonoBehaviour, IInteractable {
        [SerializeField]
        Collider2D _collider;

        public Collider2D Collider => _collider;
    }
}