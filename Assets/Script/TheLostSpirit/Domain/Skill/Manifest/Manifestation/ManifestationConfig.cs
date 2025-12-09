using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LBG;
using Sirenix.OdinInspector;
using TheLostSpirit.Identity.ConfigID;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    [Serializable]
    public class ManifestationConfig : IConfig<ManifestationConfigID>
    {
        [SerializeField, HideLabel]
        ManifestationConfigID _id;

        [SerializeReference, SubclassSelector(DrawBoxForListElements = true)]
        [ListDrawerSettings(ShowIndexLabels = true)]
        List<Effect> _effects;

        public ManifestationConfigID ID => _id;
    }

    //separate
    
    [Serializable]
    public abstract class Effect
    {
        [SerializeReference, SubclassSelector]
        ITargetSelector _targetSelector;
    }

    public class InstantDamage : Effect
    {
        [SerializeField]
        float _elementalDamage;
    }

    public interface ITargetSelector
    {
        public List<GameObject> GetTarget(Transform transform);
    }

    public class SelfSelector : ITargetSelector
    {
        public List<GameObject> GetTarget(Transform transform) {
            return new List<GameObject>();
        }
    }
}