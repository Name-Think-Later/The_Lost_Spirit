using UnityEngine;

namespace Script.TheLostSpirit.MapSystem {
    public interface IInteractable {
        public Collider2D Collider { get; }
    }
}