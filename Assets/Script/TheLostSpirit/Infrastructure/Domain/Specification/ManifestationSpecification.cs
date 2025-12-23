using System;
using System.Collections.Generic;
using MoreLinq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.SpecificationID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.Specification
{
    public class ManifestationSpecification
        : SerializedMonoBehaviour, ISpecification<ManifestationSpecificationID>, ISearchFilterable
    {
        [SerializeField]
        ManifestationSpecificationID _id;

        [OdinSerialize, HideReferenceObjectPicker]
        ManifestationConfig _config;

        [SerializeField]
        EventBindableAnimationClip _eventBindableAnimationClip;

        public ManifestationSpecificationID ID => _id;

        public ManifestationConfig Config => _config;

#if UNITY_EDITOR
        void OnValidate() {
            var frameActions = _eventBindableAnimationClip.FrameActions;
            _config.SetCombatActions(frameActions);
        }

        public bool IsMatch(string searchString) {
            var matchId = !string.IsNullOrEmpty(ID.Value) && ID.Value.Contains(searchString);

            return matchId;
        }
#endif
    }
}