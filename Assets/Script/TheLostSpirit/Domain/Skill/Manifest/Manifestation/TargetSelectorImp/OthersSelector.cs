using System.Collections.Generic;
using LBG;
using TheLostSpirit.Extension.Linq;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.TargetSelectorImp
{
    public class OthersSelector : ITargetSelector
    {
        [SerializeReference, SubclassSelector]
        EffectRange _effectRange;

        public void Initialize(Transform transform) {
            _effectRange.Initialize(transform);
        }

        public IEnumerable<IEntityMono> GetTargets() {
            return
                _effectRange
                    .Overlap()
                    .SelectComponent<IEntityMono>();
        }

#if UNITY_EDITOR
        public void DebugDrawRange(Vector2 pivot) {
            _effectRange?.DebugDraw(pivot);
        }
#endif
    }
}