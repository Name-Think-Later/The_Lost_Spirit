using R3;
using UnityEngine;

namespace Script.TheLostSpirit.MapSystem {
    public interface IInteractableReference {
        public Subject<Transform> OnInteractAsObservable { get; }
    }
}