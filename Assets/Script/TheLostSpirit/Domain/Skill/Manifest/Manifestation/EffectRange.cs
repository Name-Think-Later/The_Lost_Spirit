using System;
using TheLostSpirit.Domain.Formula;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public abstract class EffectRange
    {
        [SerializeField]
        protected Color debugColor = Color.white;

        [SerializeField]
        protected LayerMask layerMask = 0;

        [SerializeField]
        Vector2 _offset;

        protected Transform pivot;

        protected Vector2 Position => (Vector2)pivot.position + _offset;

        public abstract Collider2D[] Overlap();

        public void SetOwner(Transform owner) {
            pivot = owner;
        }

#if UNITY_EDITOR
        public abstract void DebugDraw();
#endif
    }
}