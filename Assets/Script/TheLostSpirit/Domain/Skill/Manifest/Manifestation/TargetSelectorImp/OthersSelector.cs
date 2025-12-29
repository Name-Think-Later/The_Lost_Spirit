using System.Collections.Generic;
using LBG;
using Sirenix.OdinInspector;
using TheLostSpirit.Extension.Linq;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.TargetSelectorImp
{
    public class OthersSelector : ITargetSelector
    {
        [SerializeReference, SubclassSelector, OnValueChanged(nameof(OnEffectRangeChange))]
        EffectRange _effectRange;

        Transform _owner;

        public IEnumerable<IEntityMono> GetTargets()
        {
            return
                _effectRange
                    .Overlap()
                    .SelectComponent<IEntityMono>();
        }

        public void SetOwner(Transform owner)
        {
            _owner = owner;
            _effectRange?.SetOwner(owner); // Immediately propagate to EffectRange
        }

        // Editor callback to auto-assign owner when EffectRange changes
        void OnEffectRangeChange()
        {
            _effectRange?.SetOwner(_owner);
        }

#if UNITY_EDITOR
        public void DebugDrawRange()
        {
            _effectRange?.DebugDraw();
        }
#endif
    }
}