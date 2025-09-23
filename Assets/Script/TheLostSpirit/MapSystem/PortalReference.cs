using R3;
using UnityEngine;

namespace Script.TheLostSpirit.MapSystem {
    public class PortalReference : MonoBehaviour, IInteractableReference {
        [SerializeField]
        Collider2D _collider;

        public Collider2D Collider => _collider;
        public Subject<Transform> OnInteractAsObservable { get; } = new();
    }
}