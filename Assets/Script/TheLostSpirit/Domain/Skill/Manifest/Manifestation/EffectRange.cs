using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public abstract class EffectRange
    {
        [SerializeField]
        protected Color debugColor;

        [SerializeField]
        protected LayerMask layerMask;

        protected Transform transform;

        public void Initialize(Transform transform) {
            this.transform = transform;
        }

        public abstract Collider2D[] Overlap();

#if UNITY_EDITOR
        public abstract void DebugDraw(Vector2 pivot);
#endif
    }
}