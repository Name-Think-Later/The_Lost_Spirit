using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public abstract class EffectRange
    {
        [HorizontalGroup("Column",MarginLeft = 2)]
        [SerializeField]
        protected Color debugColor = Color.white;

        [HorizontalGroup("Column", MarginLeft = 20)]
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