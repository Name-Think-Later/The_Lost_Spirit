using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using UnityEngine;

namespace TheLostSpirit.Infrastructure
{
    [Serializable]
    public class EventBindableAnimationClip
    {
        [SerializeField]
        public AnimationClip animationClip;

        public FrameActions FrameActions {
            get {
                if (!animationClip) return null;

                var events = animationClip.events;
                var rate   = animationClip.frameRate;


                var dictionary =
                    events
                        .ToDictionary(
                            e =>
                                Mathf.RoundToInt(e.time * rate),
                            e =>
                                EventDataSerializer
                                    .Deserialize(e.stringParameter)
                                    .combatActions
                        );

                return new FrameActions(dictionary);
            }
        }
    }
}