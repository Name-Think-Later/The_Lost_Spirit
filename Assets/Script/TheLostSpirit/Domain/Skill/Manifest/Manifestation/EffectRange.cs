using System;
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
        protected Vector2 offset = Vector2.zero;

        protected Vector2 GetPosition(ManifestationSubject subject) => (Vector2)subject.Transform.position + offset;

        public abstract Collider2D[] Overlap(ManifestationSubject subject);

#if UNITY_EDITOR
        public abstract void DebugDraw(Transform previewSubject);
#endif
    }
}