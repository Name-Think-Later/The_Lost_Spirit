using System.Collections.Generic;
using System.Linq;
using LBG;
using TheLostSpirit.Extension.Linq;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
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
    }
}