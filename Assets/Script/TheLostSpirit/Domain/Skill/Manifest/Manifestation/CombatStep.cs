using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using LBG;
using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectImp;
using TheLostSpirit.Extension.General;
using TheLostSpirit.Extension.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    [Serializable]
    public class CombatStep
    {
        [SerializeField]
        int _durationFrames;

        [SerializeReference, SubclassSelector]
        ITargetSelector _targetSelector;

        [SerializeReference, SubclassSelector(DrawDropdownForListElements = true)]
        [PropertySpace(SpaceAfter = 20)]
        List<Effect> _effects = new List<Effect>();

        public async UniTaskVoid Do(ManifestationSubject subject) {
            var hashset = HashSetPool<IEntityMono>.Get();

            for (var i = 0; i < _durationFrames; i++) {
                var targets = _targetSelector.GetTargets(subject);
                foreach (var target in targets) {
                    var isNewTarget = hashset.Add(target);

                    if (!isNewTarget) continue;

                    Debug.Log($"{target}".Colored(new Color(1f, 0.8f, 0.4f)) + " Got Detected");
                    foreach (var effect in _effects) {
                        effect.Apply(target, subject);
                    }
                }

                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            }

            HashSetPool<IEntityMono>.Release(hashset);
        }

#if UNITY_EDITOR
        public int DurationFrames => _durationFrames;

        public void DebugDrawRange(Transform previewSubject) {
            _targetSelector?.DebugDrawRange(previewSubject);
        }
#endif
    }
}