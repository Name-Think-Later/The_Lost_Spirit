using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Formula.Node;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Weave
{
    [Serializable]
    public class WeaveConfig : SkillConfig
    {
        [SerializeField, EnumToggleButtons]
        TraversalPolicy _traversalPolicy = TraversalPolicy.Sequential;

        [SerializeField]
        Gate _gate;

        public TraversalPolicy TraversalPolicy => _traversalPolicy;

        public Gate Gate => _gate;
    }

    [Serializable, HideLabel]
    public struct Gate
    {
        [ToggleGroup(nameof(_useGate), "Gate")]
        [SerializeField]
        bool _useGate;

        [ToggleGroup(nameof(_useGate))]
        [SerializeField]
        int _blockThreshold;

        [ToggleGroup(nameof(_useGate))]
        [SerializeField]
        int _passThreshold;
    }
}