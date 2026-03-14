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
        bool _useGate = false;

        [ToggleGroup(nameof(_useGate))]
        [SerializeField, MinValue(1)]
        int _passTime = 1;

        [ToggleGroup(nameof(_useGate))]
        [SerializeField, MinValue(1)]
        int _blockTime = 1;


        public Gate() { }

        public bool Use => _useGate && _passTime > 0 && _blockTime > 0;
        public int PassTime => _passTime;
        public int BlockTime => _blockTime;
    }
}