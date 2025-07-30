using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.TheLostSpirit.SkillSystem.SkillBase;

namespace Script.TheLostSpirit.CircuitSystem {
    partial class Circuit {
        public partial interface INode {
            public AdjacencyList Adjacencies { get; }
            public UniTaskVoid AsyncActivate();
        }
    }
}