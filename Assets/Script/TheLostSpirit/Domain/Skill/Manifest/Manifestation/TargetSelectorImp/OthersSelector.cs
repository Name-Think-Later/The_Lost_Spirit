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

        public IEnumerable<IEntityMono> GetTargets(ManifestationSubject subject)
        {
            return
                _effectRange
                    .Overlap(subject)
                    .SelectComponent<IEntityMono>();
        }

#if UNITY_EDITOR
        public void DebugDrawRange(Transform previewSubject)
        {
            _effectRange?.DebugDraw(previewSubject);
        }
#endif
    }
}